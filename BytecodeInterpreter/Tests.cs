/*
 * Tests.cs
 * ----------------------------------------
 * Contains various functions to test the
 * bytecode interpreter.
 */

using OpenTKTest.Bytecode.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Bytecode.Interpreter
{

    class Tests
    {
        public static void Main() { RunTests(); }
        
        public static void RunTests()
        {
            new VM().RunFile("example.abc");
            Console.ReadLine();
        }
    }
}