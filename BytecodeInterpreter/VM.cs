using OpenTKTest.Bytecode.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Bytecode.Interpreter
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
                        case Instruction.JUMP:
                            {
                                DataValue loc = Pop();
                                if (loc.type == DataType.Integer)
                                    reader.BaseStream.Seek(loc.intValue, SeekOrigin.Begin);
                                else
                                    InvalidDataType(instruction);
                                break;
                            }
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
                        case Instruction.RUN:
                            {
                                DataValue fileName = Pop();
                                if (fileName.type == DataType.String)
                                    RunFile(fileName.stringValue);
                                else
                                    InvalidDataType(instruction);
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
                        case Instruction.CONCATENATE:
                            {
                                DataValue[] sides = new[] { Pop(), Pop() };

                                string[] sideValues = new string[2];

                                for (int i = 1; i >= 0; --i)
                                {
                                    DataValue side = sides[i];
                                    if (side.type == DataType.String)
                                        sideValues[i] = side.stringValue;
                                    else if (side.type == DataType.Integer)
                                        sideValues[i] = side.intValue.ToString();
                                    else if (side.type == DataType.Number)
                                        sideValues[i] = side.numberValue.ToString();
                                }

                                Push(new DataValue()
                                {
                                    type = DataType.String,
                                    stringValue = sideValues[1] + sideValues[0]
                                });
                                break;
                            }
                        case Instruction.STOP:
                            {
                                reader.BaseStream.Seek(0, SeekOrigin.End);
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
