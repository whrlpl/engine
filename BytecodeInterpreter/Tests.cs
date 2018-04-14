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

        public static void TriggerRegister(ref int register, ref List<BytecodeObject> bytecodeObjects, ref bool registerModified)
        {
            bytecodeObjects.Add(new BytecodeObject()
            { 
                type = BytecodeObjectType.NumberLiteral,
                value = new IntegerExpression(register)
            });
            register = 0;
            registerModified = false;
        }

        public static void RunTests()
        {
            new VM().RunFile("example.abc");
            //var parseExp = "ConsoleLog('Test')";
            //Console.WriteLine("Test parser (" + parseExp + ")");
            //RunExpression(VM.Parse(parseExp));
            Console.ReadLine();
        }

        public static bool ConsoleLog(string logText)
        {
            // log to console test api call
            Console.WriteLine(logText);
            return true;
        }

        public static void RunExpression(Expression e)
        {
            RunExpression((NumberExpression)e);
        }

        public static void RunExpression(NumberExpression e)
        {
            double expressionResult = e.Evaluate();
            Console.WriteLine("Expression evaluated to " + expressionResult);
        }
    }
}