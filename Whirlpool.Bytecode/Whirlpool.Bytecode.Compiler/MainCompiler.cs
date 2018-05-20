/*
 * MainCompiler.cs
 * ----------------------------------------
 * The main bytecode compiler.  Work in
 * progress.
 */
using System;
using System.IO;
using Whirlpool.Bytecode.Shared;

namespace Whirlpool.Bytecode.Compiler
{
    /*  TODO: this entire bytecode programming language
     * thing needs a huge re-think and re-write
     * it isn't efficient and could definitely do with
     * something to help its design become more object
     * oriented, and something to make it much easier
     * to add to in terms of APIs and calls between 
     * the language and the game. */
    
    class MainCompiler
    {
        static void Main()
        {
            Console.WriteLine("Simple bytecode compiler");
            Console.Write("Enter the path of a file or directory of files to compile: ");
            string path = Console.ReadLine();
            if (Directory.Exists(path))
                foreach (string file in Directory.GetFiles(path))
                {
                    if (file.EndsWith(".wsc")) // whirlpool source code
                        CompileFile(file); 
                }
            else if (File.Exists(path))
                CompileFile(path);
            else
                Console.WriteLine("Could not find the directory or file " + path);

            Console.ReadLine();
        }

        static void CompileFile(string filePath)
        {
            var reader = new StreamReader(filePath);
            var writer = new ByteWriter(new FileStream(filePath.Remove(filePath.LastIndexOf(".")) + ".wcc", FileMode.Create)); // whirlpool compiled code
            var lines = reader.ReadToEnd();
            foreach (var line in lines.Split('\n'))
            {
                var words = line.Replace("\r", "").Replace("\n", "").Split(' ');
                for (int i = words.Length - 1; i >= 0; --i)
                {
                    if (i == 0)
                    {
                        foreach (var instruction in Enum.GetValues(typeof(Instruction)))
                            if (words[0] == instruction.ToString() && (Instruction)instruction != Instruction.ILIT)
                                writer.Write((Instruction)instruction);
                    }
                    else
                    {
                        writer.Write(Instruction.ILIT);
                        writer.Write(int.Parse(words[i]));
                    }
                }
            }
            writer.Close();
            reader.Close();
        }

        static bool IsLiteralString(string literalValue)
        {
            return (literalValue.StartsWith("\""));
        }
    }
}
