using OpenTKTest.Bytecode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Bytecode.Compiler
{
    public enum TokenType
    {
        Identifier,
        Keyword,
        Separator,
        Operator,
        Literal,
        Comment,
        Nothing
    }

    public enum FunctionScope
    {
        Public,
        Private,
        Protected
    }

    public class FunctionInfo
    {
        public bool valid = true;
        public string name;
        public FunctionScope scope;
        public DataType returnType;

        public static FunctionInfo Invalid = new FunctionInfo() { valid = false };
    }

    public class Token
    {
        public TokenType type;
        public string value; // Raw unmodified value

        public Token(TokenType type, string value = null)
        {
            this.type = type;
            this.value = value;
        }
    }

    public class Lexer
    {
        string[] keywords = new string[]
        {
            "pub",
            "priv",
            "prot",

            "ASM",

            "void"
        };

        string[] separators = new string[]
        {
            ";",
            "{",
            "}",
            "(",
            ")",
            ","
        };

        string[] operators = new string[]
        {
            "<",
            ">",
            "==",
            "=",
            "+",
            "-",
            "*",
            "/"
        };
        public List<Token> tokens = new List<Token>();

        public void ParseFile(string fileContents)
        {
            foreach (string s in fileContents.Split('\r'))
            {
                string line = RemoveTrailingSpaces(s.Replace("\n", "").Replace("\t", ""));
                string[] words = line.Split(' ');
                
                TokenType forceMode = TokenType.Nothing;

                foreach (var word in words)
                {
                    bool foundResult = false;
                    foreach (var keyword in keywords)
                        if (word == keyword || forceMode == TokenType.Keyword)
                        {
                            tokens.Add(new Token(TokenType.Keyword, word));
                            foundResult = true;
                        }
                    foreach (var separator in separators)
                        if (word == separator || forceMode == TokenType.Separator)
                        {
                            tokens.Add(new Token(TokenType.Separator, word));
                            foundResult = true;
                        }
                    foreach (var oper in operators)
                        if (word == oper || forceMode == TokenType.Operator)
                        {
                            tokens.Add(new Token(TokenType.Operator, word));
                            foundResult = true;
                        }

                    if (!foundResult)
                        if (word.StartsWith("//") || forceMode == TokenType.Comment)
                        {
                            tokens.Add(new Token(TokenType.Comment, word));
                        }
                        else
                        {
                            // Try parsing as a literal
                            if (int.TryParse(word, out int unused))
                                tokens.Add(new Token(TokenType.Literal, word));
                            else if (word.StartsWith("\"") || forceMode == TokenType.Literal)
                            {
                                if (forceMode == TokenType.Nothing)
                                    forceMode = TokenType.Literal;
                                tokens.Add(new Token(TokenType.Literal, word));
                            }
                            else if (word.EndsWith("\""))
                            {
                                if (forceMode == TokenType.Literal)
                                    forceMode = TokenType.Nothing;
                                tokens.Add(new Token(TokenType.Literal, word));
                            }
                            else
                                foreach (var t in ReadThroughIdentifier(word))
                                    tokens.Add(t);
                        }                            
                }
            }
        }

        public void SyntaxError(string line)
        {
            throw new Exception("Syntax error on line: \n\t" + line);
        }

        public string GetFunctionName(string s)
        {
            if (s.IndexOf('(') >= s.IndexOf(')')) throw new Exception("what the fuck");

            return s.Remove(s.IndexOf('('));
        }

        public Token[] ReadThroughIdentifier(string identifier)
        {
            // Read through an identifier, checking for keywords & separators
            List<Token> tokens = new List<Token>();
            if (string.IsNullOrEmpty(identifier)) return tokens.ToArray();
            int lastIndex = 0;

            for (int i = 0; i < identifier.Length; ++i)
            {
                bool foundSeparator = false;
                foreach (string separator in separators)
                {
                    string totalSeparator = "";
                    for (int o = 0; o < separator.Length; ++o)
                    {
                        if (identifier[i] == separator[o])
                        {
                            totalSeparator += identifier[i];
                            tokens.Add(new Token(TokenType.Identifier, identifier.Remove(0, lastIndex).Remove(i - lastIndex)));
                            tokens.Add(new Token(TokenType.Separator, totalSeparator));
                            lastIndex = i;
                            foundSeparator = true;
                        }
                    }
                }
                if (!foundSeparator)
                    foreach (string oper in operators)
                    {
                        string totalOperator = "";
                        for (int o = 0; o < oper.Length; ++o)
                        {
                            if (identifier[i] == oper[o])
                            {
                                totalOperator += identifier[i];
                                tokens.Add(new Token(TokenType.Identifier, identifier.Remove(0, lastIndex).Remove(i - lastIndex)));
                                tokens.Add(new Token(TokenType.Operator, totalOperator));
                                lastIndex = i;
                            }
                        }
                    }
            }

            if (lastIndex < identifier.Length)
                tokens.Add(new Token(TokenType.Identifier, identifier.Remove(0, lastIndex + 1)));

            if (tokens.Count < 1 && !string.IsNullOrWhiteSpace(identifier)) tokens.Add(new Token(TokenType.Identifier, identifier));

            return tokens.ToArray();
        }
            
        public string RemoveTrailingSpaces(string s)
        {
            string line = s;
            while (line.StartsWith(" "))
                line = line.Remove(0, 1);
            while (line.EndsWith(" "))
                line = line.Remove(line.Length - 1);

            return line;
        }

    }
}
