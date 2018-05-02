using System;
using System.IO;
using Whirlpool.Bytecode.Shared;

namespace Whirlpool.Bytecode.Compiler
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
}
