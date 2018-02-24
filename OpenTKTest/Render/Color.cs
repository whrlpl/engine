using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Render
{
    public class Color
    {
        public int R;
        public int G;
        public int B;
        public int A;

        public Color(Color c, int A_)
        {
            R = c.R;
            G = c.G;
            B = c.B;
            A = A_;                
        }

        public Color(int R_, int G_, int B_, int A_ = 255)
        {
            R = R_;
            G = G_;
            B = B_;
            A = A_;
        }

        public static Color Black = new Color(0, 0, 0);
        public static Color Red = new Color(255, 0, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);
        public static Color White = new Color(255, 255, 255);
    }
}
