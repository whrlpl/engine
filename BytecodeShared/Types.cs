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
        String,
        Boolean
    }

    public class DataValue
    {
        public DataType type;

        public int intValue;
        public double numberValue;
        public string stringValue;
        public bool boolValue;
    }
    
    public enum ArithmeticOperator
    {
        Multiply,
        Divide,
        Add,
        Subtract,
        Unknown
    }
}
