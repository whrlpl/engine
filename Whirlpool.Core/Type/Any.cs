using OpenTK;
using OpenTK.Graphics;
using System;

namespace Whirlpool.Core.Type
{
    public class Any
    {        
        public object value;
        public System.Type valueType;

        public Any(object value)
        {
            this.value = value;
            this.valueType = value.GetType();
        }

        public dynamic GetValue() => Convert.ChangeType(value, valueType);

        public static implicit operator Any(bool value) => new Any(value);
        public static implicit operator Any(float value) => new Any(value);
        public static implicit operator Any(int value) => new Any(value);
        public static implicit operator Any(Vector2 value) => new Any(value);
        public static implicit operator Any(Vector3 value) => new Any(value);
        public static implicit operator Any(Color4 value) => new Any(value);
        public static implicit operator Any(Matrix4 value) => new Any(value);
    }
}
