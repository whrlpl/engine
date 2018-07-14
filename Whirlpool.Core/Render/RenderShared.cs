using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Render
{
    public class RenderShared
    {
        public static Vector2 renderResolution = new Vector2(-1, -1);

        public static void Init()
        {
            if (renderResolution.X < 0 || renderResolution.Y < 0)
            {
                renderResolution = new Vector2(BaseGame.Size.Width, BaseGame.Size.Height);
            }
            Render3D.Init();
            Render2D.Init();
        }
    }
}
