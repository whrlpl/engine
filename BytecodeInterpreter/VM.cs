using OpenTKTest.Bytecode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Bytecode.Interpreter
{
    class VM
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
            using (var stream = new FileStream(file, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        Instruction instruction = (Instruction)reader.ReadByte();
                        switch (instruction)
                        {
                            case Instruction.INTEGER_LITERAL:
                                Push(new DataValue()
                                {
                                    type = DataType.Integer,
                                    intValue = reader.ReadInt32()
                                });
                                break;
                            case Instruction.NUMBER_LITERAL:
                                Push(new DataValue() {
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
                            case Instruction.CONSOLELOG:
                                {
                                    DataValue val = Pop();
                                    if (val.type == DataType.String)
                                        Console.WriteLine(val.stringValue);
                                    else if (val.type == DataType.Number)
                                        Console.WriteLine(val.numberValue);
                                    else if (val.type == DataType.Integer)
                                        Console.WriteLine(val.intValue);
                                    else
                                        InvalidDataType(instruction);
                                    break;
                                }
                            case Instruction.ADD:
                                {
                                    DataValue left = Pop();
                                    DataValue right = Pop();

                                    if (left.type == DataType.Integer)
                                        Push(new DataValue() {
                                            type = DataType.Integer,
                                            intValue = left.intValue + right.intValue
                                        });
                                    else if (left.type == DataType.Number)
                                        Push(new DataValue()
                                        {
                                            type = DataType.Number,
                                            numberValue = left.numberValue + right.numberValue
                                        });
                                    else
                                        InvalidDataType(instruction);
                                    break;
                                }
                            default:
                                Console.WriteLine("Unrecognized instruction " + instruction.ToString() + " at position " + reader.BaseStream.Position);
                                break;
                        }
                    }
                }
            }                
        }
    }
}
