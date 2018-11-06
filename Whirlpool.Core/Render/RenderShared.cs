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

        public static void Init(Vector2 desiredResolution)
        {
            renderResolution = desiredResolution;
            if (desiredResolution.X < 0 || desiredResolution.Y < 0)
            {
                renderResolution = new Vector2(BaseGame.Size.Width, BaseGame.Size.Height);
            }
            Renderer3D.Init();
            Renderer2D.Init();
        }
    }
}
