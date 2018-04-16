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
    }
}
