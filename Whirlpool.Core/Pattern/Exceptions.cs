using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Pattern
{
    [Serializable]
    public class TextureNotFoundException : Exception
    {
        public TextureNotFoundException(string message) : base(message) { }
    }

    [Serializable]
    public class TextureAlreadyExistsException : Exception
    {
        public TextureAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class TextureLoadException : Exception
    {
        public TextureLoadException(string message) : base(message) { }
    }
}
