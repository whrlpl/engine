using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Type
{
    public class Rectangle
    {
        public float x = 0;
        public float y = 0;
        public float width = 0;
        public float height = 0;

        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool Contains(Vector2 pos)
        {
            Console.WriteLine(pos.X + "," + pos.Y + " : " + x + "," + y  + " < " + (x + width) + "," + (y + height));
            return (
                    (pos.X >= x && pos.Y >= y)
                    &&
                    (pos.X <= x + width && pos.Y <= y + height)
                );
            /* 
             * x0y0                          x200y0
             * |----------------------------------|
             * |                                  |
             * |                                  |
             * |        R E C T A N G L E         |
             * |                                  |
             * |                                  |
             * |----------------------------------|
             * x0y100                      x200y100
             */
        }
    }
}
