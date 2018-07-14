using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Type
{
    public class Any
    {        
        public object value;
        public System.Type valueType;

        public Any(System.Type valueType, object value)
        {
            this.value = value;
            this.valueType = valueType;
        }

        public dynamic GetValue() => Convert.ChangeType(value, valueType);

        public static implicit operator Any(Matrix4 value) => new Any(value.GetType(), value);
        public static implicit operator Any(float value) => new Any(value.GetType(), value);
        public static implicit operator Any(int value) => new Any(value.GetType(), value);
        public static implicit operator Any(Vector2 value) => new Any(value.GetType(), value);
        public static implicit operator Any(bool value) => new Any(value.GetType(), value);
        public static implicit operator Any(string value) => new Any(value.GetType(), value);
    }
}
