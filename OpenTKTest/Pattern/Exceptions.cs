using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Pattern
{
    public class TextureNotFoundException : Exception
    {
        public TextureNotFoundException(string message) : base(message)
        {
        }
    }

    public class TextureAlreadyExistsException : Exception
    {
        public TextureAlreadyExistsException(string message) : base(message)
        {
        }
    }

    public class TextureLoadException : Exception
    {
        public TextureLoadException(string message) : base(message)
        {
        }
    }
}
