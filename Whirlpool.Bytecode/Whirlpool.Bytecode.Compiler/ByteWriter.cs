/*
 * ByteWriter.cs
 * ----------------------------------------
 * Contains the ByteWriter class - extends
 * upon the BinaryWriter class allowing it
 * to write Insturctions directly, and 
 * logging everything written.
 */
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
            Console.WriteLine("Wrote instruction " + value.ToString());
            base.Write((byte)value);
        }

        public override void Write(byte value)
        {
            Console.WriteLine("Wrote byte " + value);
            base.Write(value);
        }

        public override void Write(int value)
        {
            Console.WriteLine("Wrote integer " + value);
            base.Write(value);
        }

        public override void Write(string value)
        {
            Console.WriteLine("Wrote string " + value);
            base.Write(value);
        }
    }
}
