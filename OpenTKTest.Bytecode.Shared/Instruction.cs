using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Bytecode.Shared
{
    public enum Instruction : byte
    {
        NUMBER_LITERAL = 1,
        INTEGER_LITERAL,
        STRING_LITERAL,

        OUTPUT,
        INPUT,

        DECLARE_VARIABLE,
        GOTO,

        ADD,
        MULTIPLY,
        DIVIDE,
        SUBTRACT,

        GETSTACKSIZE,
        PUSH,
        POP
    }
}
