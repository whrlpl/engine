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
    /* TODO: this entire bytecode programming language
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

        static void DecompileFile(string filePath)
        {
            // reverses the compilation process
            // decompiles into ASM
            var writer = new ByteWriter(new FileStream(filePath.Remove(filePath.LastIndexOf(".")) + ".decompiled.wsc", FileMode.Create)); // whirlpool compiled code
            var memStream = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memStream);
            }
            var reader = new BinaryReader(memStream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var instruction = (Instruction)reader.ReadByte();
                Console.WriteLine(instruction.ToString());
                //switch ()
                //{
                //    case Instruction.NLIT:
                //        Push(new DataValue()
                //        {
                //            type = DataType.Number,
                //            numberValue = reader.ReadDouble()
                //        });
                //        break;
                //    case Instruction.ILIT:
                //        Push(new DataValue()
                //        {
                //            type = DataType.Integer,
                //            intValue = reader.ReadInt32()
                //        });
                //        break;
                //    case Instruction.SLIT:
                //        Push(new DataValue()
                //        {
                //            type = DataType.String,
                //            stringValue = reader.ReadString()
                //        });
                //        break;
                //    case Instruction.OUT:
                //        {
                //            var text = Pop();
                //            if (text.type != DataType.Integer || text.intValue < 0 || text.intValue > reader.BaseStream.Length)
                //                Console.WriteLine("Decompilation error");
                //            Console.WriteLine(text.intValue);
                //            break;
                //        }
                //    case Instruction.IN:
                //        throw new NotImplementedException();
                //        break;
                //    case Instruction.LD:
                //        {
                //            var location = Pop();
                //            if (location.type != DataType.Integer || location.intValue < 0 || location.intValue > reader.BaseStream.Length)
                //                Console.WriteLine("Decompilation error");
                //            Push(stack[location.intValue]);
                //            break;
                //        }
                //    case Instruction.JMP:
                //        {
                //            var location = Pop();
                //            if (location.type != DataType.Integer || location.intValue < 0 || location.intValue > reader.BaseStream.Length)
                //                Console.WriteLine("Decompilation error");
                //            reader.BaseStream.Position = location.intValue;
                //            break;
                //        }
                //    case Instruction.GSS:
                //        {
                //            Push(new DataValue()
                //            {
                //                type = DataType.Integer,
                //                intValue = stackSize
                //            });
                //            break;
                //        }
                //    default:
                //        Console.WriteLine("Unrecognized instruction " + instruction.ToString() + " at position " + reader.BaseStream.Position);
                //        break;
                //}

            }
            writer.Close();
            reader.Close();
            reader.Close();
            memStream.Close();
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
