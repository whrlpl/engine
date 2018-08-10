using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Type
{
    public static class VectorExt
    {
        public static float LengthWithSign(this Vector3 vector)
        {
            var polarity = 1.0f;
            if (vector.X < 0 || vector.Y < 0 || vector.Z < 0)
                polarity = -1.0f;
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z) * polarity;
        }

        public static Vector2 Divide(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }
    }
}
