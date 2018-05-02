using System;
using System.Collections.Generic;
using System.Text;
using Whirlpool.Bytecode.Shared;

namespace Whirlpool.Bytecode.Compiler
{
    class WriteQueue
    {
        public List<byte> byteQueue = new List<byte>();

        public void Write(Instruction i)
        {
            Insert(byteQueue.Count, i);
        }

        public void Write(byte b)
        {
            Insert(byteQueue.Count, b);
        }

        public void Write(string s)
        {
            Insert(byteQueue.Count, s);
        }

        public void Write(int i)
        {
            Insert(byteQueue.Count, i);
        }

        public void Insert(int pos, byte b)
        {
            byteQueue.Insert(pos, b);
        }

        public void Insert(int pos, string s)
        {
            List<byte> bytes = new List<byte>();
            foreach (char c in s)
            {
                bytes.Add(Encoding.ASCII.GetBytes(new[] { c })[0]);
            }
            bytes.Insert(0, (byte)s.Length);
            for (int i_ = 0; i_ < bytes.Count; ++i_)
                Insert(pos + i_, bytes[i_]);
        }

        public void Insert(int pos, Instruction i)
        {
            Insert(pos, (byte)i);
        }

        public void Insert(int pos, int i)
        {
            var bytes = BitConverter.GetBytes(i);
            for (int i_ = 0; i_ < bytes.Length; ++i_)
                Insert(pos + i_, bytes[i_]);
        }
    }
}
