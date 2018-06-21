/*
 * VM.cs
 * ----------------------------------------
 * The Virtual Machine for the running and
 * operation of the bytecode language.
 */
using System;
using System.IO;
using Whirlpool.Bytecode.Shared;

namespace Whirlpool.Script.Interpreter
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
            BinaryReader reader = new BinaryReader(memStream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                ExecInstruction(ref reader, (Instruction)reader.ReadByte());
            }
            reader.Close();
            memStream.Close();
        }

        public void ExecInstruction(ref BinaryReader reader, Instruction instruction)
        {
            switch (instruction)
            {
                case Instruction.NLIT:
                    Push(new DataValue()
                    {
                        type = DataType.Number,
                        numberValue = reader.ReadDouble()
                    });
                    break;
                case Instruction.ILIT:
                    Push(new DataValue()
                    {
                        type = DataType.Integer,
                        intValue = reader.ReadInt32()
                    });
                    break;
                case Instruction.SLIT:
                    Push(new DataValue()
                    {
                        type = DataType.String,
                        stringValue = reader.ReadString()
                    });
                    break;
                case Instruction.OUT:
                    {
                        var text = Pop();
                        if (text.type != DataType.Integer)
                            InvalidDataType(instruction);
                        Console.WriteLine(text.intValue);
                        break;
                    }
                case Instruction.IN:
                    throw new NotImplementedException();
                case Instruction.LD:
                    {
                        var location = Pop();
                        if (location.type != DataType.Integer || location.intValue < 0 || location.intValue > reader.BaseStream.Length)
                            InvalidDataType(instruction);
                        Push(stack[location.intValue]);
                        break;
                    }
                case Instruction.JMP:
                    {
                        var location = Pop();
                        if (location.type != DataType.Integer || location.intValue < 0 || location.intValue > reader.BaseStream.Length)
                            InvalidDataType(instruction);
                        reader.BaseStream.Position = location.intValue;
                        break;
                    }
                case Instruction.GSS:
                    {
                        Push(new DataValue()
                        {
                            type = DataType.Integer,
                            intValue = stackSize
                        });
                        break;
                    }
                case Instruction.ADD:
                    {
                        var numberLeft = Pop();
                        var numberRight = Pop();
                        if (numberLeft.type != DataType.Integer || numberRight.type != DataType.Integer)
                            InvalidDataType(instruction);
                        Push(new DataValue()
                        {
                            type = DataType.Integer,
                            intValue = numberLeft.intValue + numberRight.intValue
                        });
                        break;
                    }
                default:
                    Console.WriteLine("Unrecognized instruction " + instruction.ToString() + " at position " + reader.BaseStream.Position);
                    break;
            }
        }
    }
}
