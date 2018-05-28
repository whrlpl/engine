/*
 * Tests.cs
 * ----------------------------------------
 * Contains various functions to test the
 * bytecode interpreter.
 */
using System;

namespace Whirlpool.Script.Interpreter
{

    class Tests
    {
        public static void Main() => RunTests();
        
        public static void RunTests()
        {
            new VM().RunFile("example.wcc");
            Console.ReadLine();
        }
    }
}