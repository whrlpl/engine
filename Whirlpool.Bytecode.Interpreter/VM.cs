using Whirlpool.Bytecode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Bytecode.Interpreter
{
    public class VM
    {
        static int maxStackSize = 256;
        public int stackSize = 0;
        public DataValue[] stack = new DataValue[maxStackSize];

        public void Push(DataValue value)
        {
            if (stackSize >= maxStackSize) throw new Exception("Attempted to push to a full stack.");
            stack[stackSize++] = value;
        }

        public DataValue Pop()
        {
            if (stackSize <= 0) throw new Exception("Attempted to pop from an empty stack.");
            return stack[--stackSize];
        }

        private void InvalidDataType(Instruction instruction)
        {
            throw new Exception("Wrong data type for instruction " + instruction);
        }

        public void RunFile(string file)
        {
            MemoryStream memStream = new MemoryStream();
            using (var stream = new FileStream(file, FileMode.Open))
            {
                stream.CopyTo(memStream);
            }
            using (var reader = new BinaryReader(memStream))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    Instruction instruction = (Instruction)reader.ReadByte();
                    switch (instruction)
                    {
                        // TODO: make this more object oriented
                        // TODO: put this in Bytecode.Shared
                        case Instruction.INTEGER_LITERAL:
                            Push(new DataValue()
                            {
                                type = DataType.Integer,
                                intValue = reader.ReadInt32()
                            });
                            break;
                        case Instruction.NUMBER_LITERAL:
                            Push(new DataValue()
                            {
                                type = DataType.Number,
                                numberValue = reader.ReadDouble()
                            });
                            break;
                        case Instruction.STRING_LITERAL:
                            Push(new DataValue()
                            {
                                type = DataType.String,
                                stringValue = reader.ReadString()
                            });
                            break;
                        case Instruction.GOTO:
                            {
                                var location = Pop();
                                if (location.type != DataType.Integer || location.intValue < 0 || location.intValue > reader.BaseStream.Length)
                                    InvalidDataType(instruction);
                                reader.BaseStream.Position = location.intValue;
                                break;
                            }
                        case Instruction.OUTPUT:
                            {
                                var str = Pop();
                                if (str.type != DataType.String)
                                    InvalidDataType(instruction);
                                Console.WriteLine(str);
                                break;
                            }
                        case Instruction.GETSTACKSIZE:
                            {
                                Push(new DataValue()
                                {
                                    type = DataType.Integer,
                                    intValue = stackSize
                                });
                                break;
                            }
                        default:
                            Console.WriteLine("Unrecognized instruction " + instruction.ToString() + " at position " + reader.BaseStream.Position);
                            break;
                    }
                }
            }
            memStream.Close();
        }
    }
}
