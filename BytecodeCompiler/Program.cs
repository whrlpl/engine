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

        public override void Write(byte value)
        {
            Console.WriteLine("Wrote " + value);
            base.Write(value);
        }
    }

    class FunctionInfo
    {
        public bool valid = true;
        public string name;

        public static FunctionInfo Invalid = new FunctionInfo() { valid = false };
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
                string newFileName = filePath.Remove(filePath.LastIndexOf('.')) + ".abc"; // agthrs bytecode ;)
                string fileContents = reader.ReadToEnd();
                string[] lines = fileContents.Split('\r');

                var writer = new ByteWriter(new FileStream(newFileName, FileMode.Create));

                foreach (string s in lines)
                {
                    string[] words = new string[2];

                    if (s.Contains(" "))
                    {
                        // split
                        words[0] = s.Remove(s.IndexOf(' '));
                        words[1] = s.Remove(0, s.IndexOf(' ') + 1);
                    }
                    else
                    {
                        words[0] = s;
                    }

                    Instruction instructionOpcode = Instruction.NULL;
                    for (int i = 0; i < words.Length; ++i)
                    {
                        string word = words[i];
                        if (word == null) continue;
                        word = word.Replace("\n", "").Replace("\r", ""); // removes newlines just in case
                        if (i == 0)
                        {
                            // Opcode
                            bool instructionFound = false;
                            foreach (var instruction in Enum.GetValues(typeof(Instruction)))
                            {
                                if (instruction.ToString() == word)
                                {
                                    writer.Write((byte)instruction);
                                    instructionOpcode = (Instruction)instruction;
                                    instructionFound = true;
                                }
                            }
                            if (!instructionFound)
                                Console.WriteLine("Error: " + word + " is not a valid instruction");
                        }
                        else
                        {
                            // Operand
                            // Recognize operand type
                            if (word.StartsWith("\""))
                            {
                                writer.Write(word.Remove(0, 1).Remove(word.Length - 2));
                            }
                            else
                            {
                                writer.Write(int.Parse(word.Replace(" ", ""))); // number
                            }
                        }
                    }
                }

                writer.Close();
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
