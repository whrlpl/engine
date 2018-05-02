using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Bytecode.Compiler
{
    class Function
    {
        public string name;
        public int location;
        public int parameters;
        public bool builtIn;

        public EventHandler<Tuple<Lexer, int>> builtInFunc;
    }
}
