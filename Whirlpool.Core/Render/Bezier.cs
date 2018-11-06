using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Render
{
    public static class Bezier
    {
        public static float Lerp(float p0, float p1, float t)
        {
            return (1 - t) * p0 + t * p1;
        }

        public static float QuadraticBezierCurve(float p0, float p1, float p2, float t)
        {
            float b0 = Lerp(p0, p1, t);
            float b1 = Lerp(p1, p2, t);
            return Lerp(b0, b1, t);
        }
        public static float BezierCurve(float p0, float p1, float p2, float p3, float t)
        {
            return Lerp(QuadraticBezierCurve(p0, p1, p2, t), QuadraticBezierCurve(p1, p2, p3, t), t);
        }

    }
}
