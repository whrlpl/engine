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
        
        public Vector2 xy { get { return new Vector2(x, y); } set { x = value.X; y = value.Y; } }
        public Vector2 wh { get { return new Vector2(width, height); } set { width = value.X; height = value.Y; } }
        public Vector2 size { get { return wh; } set { wh = value; } }


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

        public Rectangle(Vector2 xy, Vector2 wh)
        {
            this.xy = xy;
            this.wh = wh;
        }

        /// <summary>
        /// Check whether a rectangle contains a point.
        /// </summary>
        /// <param name="pos">The point to check for</param>
        /// <returns>Whether the rectangle contains a point</returns>
        public bool Contains(Vector2 pos)
        {
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

        public override string ToString()
        {
            return this.x + ", " + this.y + ", " + this.width + ", " + this.height;
        }

        public static Rectangle operator + (Rectangle a, Rectangle b)
        {
            return new Rectangle(a.x + b.x, a.y + b.y, a.width + b.width, a.height + b.height);
        }

        public static Rectangle operator +(Rectangle a, Vector2 b)
        {
            return new Rectangle(a.x + b.X, a.y + b.Y, a.width, a.height);
        }
    }
}
