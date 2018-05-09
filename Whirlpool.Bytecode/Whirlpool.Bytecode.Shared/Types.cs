/*
 * Types.cs
 * ----------------------------------------
 * Contains several classes used in the VM
 * and bytecode language.
 */

namespace Whirlpool.Bytecode.Shared
{
    public enum DataType
    {
        Integer,
        Number,
        String,
        Boolean,
        Void
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
