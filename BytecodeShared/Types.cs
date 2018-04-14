using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Bytecode.Shared
{
    public enum DataType
    {
        Integer,
        Number,
        String
    }

    public class DataValue
    {
        public DataType type;

        public int intValue;
        public double numberValue;
        public string stringValue;
    }


    public enum BytecodeObjectType
    {
        NumberLiteral,
        ArithmeticOperator,
        Keyword,
        Unknown
    }

    public enum ArithmeticOperator
    {
        Multiply,
        Divide,
        Add,
        Subtract,
        Unknown
    }

    public class BytecodeObject
    {
        public BytecodeObjectType type;
        public object value;
    }

}
