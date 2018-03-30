using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Render
{
    public class Camera
    {
        Vector3 position = new Vector3(0, 0, 0);
        Matrix4 view
        {
            get
            {
                return Matrix4.LookAt(position, new Vector3(position.X, position.Y, position.Z - 4.0f), new Vector3(0.0f, 1.0f, 0.0f));
            }
        }
        Matrix4 projection
        {
            get
            {
                return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), windowRatio, 0.1f, 100.0f);
            }
        }

        public float windowRatio
        {
            get
            {
                int[] viewport = new int[4];
                GL.GetInteger(GetPName.Viewport, viewport);
                return (float)viewport[2] / viewport[3];
            }
        }
        public Matrix4 vp
        {
            get
            {
                return view * projection;
            }
        }

        public void Update()
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Key.W))
            {
                position.Z -= 0.01f;
            }
            if (keyboardState.IsKeyDown(Key.S))
            {
                position.Z += 0.01f;
            }
            if (keyboardState.IsKeyDown(Key.A))
            {
                position.X -= 0.01f;
            }
            if (keyboardState.IsKeyDown(Key.D))
            {
                position.X += 0.01f;
            }
        }
    }
}
