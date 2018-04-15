using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKTest.Bytecode.Shared;

namespace OpenTKTest.Bytecode.Compiler
{
    class ByteWriter : BinaryWriter
    {
        public ByteWriter(Stream output) : base(output) { }

        public void Write(Instruction value)
        {
            Console.WriteLine("Wrote " + value);
            base.Write((byte)value);
        }

        public override void Write(byte value)
        {
            Console.WriteLine("Wrote " + value);
            base.Write(value);
        }

        public override void Write(int value)
        {
            Console.WriteLine("Wrote " + value);
            base.Write(value);
        }

        public override void Write(string value)
        {
            Console.WriteLine("Wrote " + value);
            base.Write(value);
        }
    }


    class Program
    {
        static void Main()
        {
            Console.WriteLine("Simple bytecode compiler");
            Console.Write("Enter the path of a file or directory of files to compile: ");
            string path = Console.ReadLine();
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (file.EndsWith(".asc")) // agthrs source code ;)
                    {
                        CompileFile(file);
                    }
                }
            }
            else if (File.Exists(path))
            {
                CompileFile(path);
            }
            else
            {
                Console.WriteLine("Could not find the directory or file " + path);
            }

            Console.ReadLine();
        }

        static void CompileFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var lexer = new Lexer();
                lexer.ParseFile(reader.ReadToEnd());

                foreach (var token in lexer.tokens)
                    Console.WriteLine(token.type + ": " + token.value);
            }
        }

        static FunctionInfo GetFunctionFromString(string str)
        {
            // parse the string out
            if (str.IndexOf('(') >= str.IndexOf(')')) return FunctionInfo.Invalid;

            FunctionInfo functionInfo = new FunctionInfo();

            functionInfo.name = str.Remove(str.IndexOf('('));

            return functionInfo;
        }

        static void TriggerNumberRegister(ref int register, ref ByteWriter writer, ref bool registerModified)
        {
            Console.WriteLine((byte)register);
            writer.Write((byte)Instruction.NUMBER_LITERAL);
            writer.Write(register);
            register = 0;
            registerModified = false;
        }

        static ArithmeticOperator GetOperator(char o)
        {
            switch (o)
            {
                case '*':
                    return ArithmeticOperator.Multiply;
                case '/':
                    return ArithmeticOperator.Divide;
                case '+':
                    return ArithmeticOperator.Add;
                case '-':
                    return ArithmeticOperator.Subtract;
                default:
                    return ArithmeticOperator.Unknown;
            }
        }
    }
}
