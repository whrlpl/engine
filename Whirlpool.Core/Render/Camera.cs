using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;

namespace Whirlpool.Core.Render
{
    public class Camera
    {
        public Vector3 position = new Vector3(0, 0, 10);

        public float vAngle;
        public float hAngle;

        Vector3 cameraFront
        {
            get
            {
                return new Vector3((float)(Math.Cos(vAngle) * Math.Sin(hAngle)),
                    (float)Math.Sin(vAngle),
                    (float)(Math.Cos(vAngle) * Math.Cos(hAngle)));
            }
        }

        Vector3 cameraUp = new Vector3(0.0f, 1.0f, 0.0f);

        /// <summary>
        /// View matrix for camera matrix calculations.
        /// </summary>
        Matrix4 view
        {
            get
            {
                return Matrix4.LookAt(position, new Vector3(0, 0, 0), cameraUp);
            }
        }

        /// <summary>
        /// Projection matrix for camera matrix calculations.
        /// </summary>
        Matrix4 projection
        {
            get
            {
                return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), windowRatio, 0.1f, 100.0f);
            }
        }

        /// <summary>
        /// The ratio for window width to window height.
        /// </summary>
        public float windowRatio
        {
            get
            {
                int[] viewport = new int[4];
                GL.GetInteger(GetPName.Viewport, viewport);
                return (float)viewport[2] / viewport[3];
            }
        }

        /// <summary>
        /// ViewProjection matrix for camera matrix calculations.
        /// </summary>
        public Matrix4 vp
        {
            get
            {
                return projection * view;
            }
        }

        /// <summary>
        /// Update the camera.
        /// </summary>
        public void Update() { }
    }
}
