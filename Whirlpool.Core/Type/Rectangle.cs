using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;

namespace Whirlpool.Core.Type
{
    public class Rectangle
    {
        public float x = 0;
        public float y = 0;
        public float width = 0;
        public float height = 0;

        /// <summary>
        /// Initializes a rectangle.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="width">The relative width</param>
        /// <param name="height">The relative height</param>
        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Check whether a rectangle contains a point.
        /// </summary>
        /// <param name="pos">The point to check for</param>
        /// <returns>Whether the rectangle contains a point</returns>
        public bool Contains(Vector2 pos)
        {
            Logging.Write(pos.X + "," + pos.Y + " : " + x + "," + y  + " < " + (x + width) + "," + (y + height));
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
