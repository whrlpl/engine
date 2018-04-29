using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;

namespace Whirlpool.Game.Logic
{
    public class LocalPlayer
    {
        public Camera camera;
        public Vector3 position;
        public Vector3 rotation;
        
        public void Init()
        {
            position = rotation = Vector3.Zero;
            camera = new Camera();
        }

        public void Update()
        {
            if (InputHandler.GetStatus().keyboardKeys['w'])
            {
                position.Y -= 0.01f;
            }
            if (InputHandler.GetStatus().keyboardKeys['a'])
            {
                position.X -= 0.01f;
            }
            if (InputHandler.GetStatus().keyboardKeys['s'])
            {
                position.Y += 0.01f;
            }
            if (InputHandler.GetStatus().keyboardKeys['d'])
            {
                position.X += 0.01f;
            }
        }
    }
}
