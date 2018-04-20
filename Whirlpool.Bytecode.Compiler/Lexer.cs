using Whirlpool.Bytecode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Whirlpool.Bytecode.Compiler
{
    public enum TokenType
    {
        Identifier,
        Keyword,
        Separator,
        Operator,
        Literal,
        Comment
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

    public class TokenMatch
    {
        public TokenType type;
        public string value;
        public string remainingText;
        public bool matched;
    }    

    public class Token
    {
        public TokenType type;
        public string pattern; // Raw unmodified value

        public Token(TokenType type, string pattern = null)
        {
            this.type = type;
            this.pattern = pattern;
        }

        public TokenMatch GetMatch(string str)
        {
            var regexMatch = Regex.Match(str, pattern);
            if (regexMatch.Success)
            {
                string remainingText = "";
                if (regexMatch.Value.Length != str.Length)
                    remainingText = str.Remove(0, regexMatch.Value.Length);

                return new TokenMatch()
                {
                    matched = true,
                    type = type,
                    value = regexMatch.Value,
                    remainingText = remainingText
                };
            }
            else
            {
                return new TokenMatch()
                {
                    matched = false
                };
            }
        }
    }

    public class Lexer {
        public List<Token> tokenDefinitions = new List<Token>()
        {
            new Token(TokenType.Keyword, "^pub"),
            new Token(TokenType.Keyword, "^priv"),
            new Token(TokenType.Keyword, "^prot"),
            new Token(TokenType.Keyword, "^func"),
            new Token(TokenType.Keyword, "^ASM"),
            new Token(TokenType.Keyword, "^for"),
            new Token(TokenType.Keyword, "^let"),
            new Token(TokenType.Keyword, "^ret"),
            new Token(TokenType.Keyword, "^dep"),

            new Token(TokenType.Keyword, "^void"),

            new Token(TokenType.Separator, "^;"),
            new Token(TokenType.Separator, "^{"),
            new Token(TokenType.Separator, "^}"),
            new Token(TokenType.Separator, "^\\("),
            new Token(TokenType.Separator, "^\\)"),
            new Token(TokenType.Separator, "^,"),

            new Token(TokenType.Operator, "^<"),
            new Token(TokenType.Operator, "^>"),
            new Token(TokenType.Operator, "^-="),
            new Token(TokenType.Operator, "^+="),
            new Token(TokenType.Operator, "^=="),
            new Token(TokenType.Operator, "^!="),
            new Token(TokenType.Operator, "^="),
            new Token(TokenType.Operator, "^\\+"),
            new Token(TokenType.Operator, "^-"),
            new Token(TokenType.Operator, "^\\*"),
            new Token(TokenType.Operator, "^/"),

            new Token(TokenType.Literal, "^\"[^\"]*\""),
            new Token(TokenType.Literal, "^\\d+"),
            
            new Token(TokenType.Identifier, "^\\w+")
        };

        public List<TokenMatch> tokens = new List<TokenMatch>();

        public void ParseFile(string fileContents)
        {
            foreach (string line in fileContents.Split('\r'))
            {
                string text = line;
                while (!string.IsNullOrWhiteSpace(text))
                {
                    var matchedStr = false;
                    foreach (Token tokenDef in tokenDefinitions)
                    {
                        TokenMatch tokenMatch = tokenDef.GetMatch(text);
                        if (tokenMatch.matched)
                        {
                            tokens.Add(tokenMatch);
                            text = tokenMatch.remainingText;
                            matchedStr = true;
                        }
                    }
                    if (!matchedStr)
                    {
                        if (text.Length > 0)
                            text = text.Remove(0, 1);
                    }
                }
            }            
        }

        public void SyntaxError(string line)
        {
            throw new Exception("Syntax error on line: \n\t" + line);
        }
    }
}
