using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using Whirlpool.Core.IO;

namespace Whirlpool.Core.Render
{
    public class Camera
    {
        public Vector3 position = new Vector3(10, 2, 0);
        public Vector3 lookAtPos = new Vector3(-10, 0, 0);

        public Vector2 viewportSize;
        public float fieldOfView = 50;
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
        public Matrix4 view
        {
            get
            {
                var rotationVector = new Vector3((float)Math.Sin(Time.currentTime) * 4, 0, (float)Math.Cos(Time.currentTime) * 4);
                return Matrix4.LookAt(position, lookAtPos, cameraUp);
            }
        }

        /// <summary>
        /// Projection matrix for camera matrix calculations.
        /// </summary>
        public Matrix4 projection
        {
            get
            {
                return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), windowRatio, 0.1f, 250.0f);
            }
        }

        /// <summary>
        /// The ratio for window width to window height.
        /// </summary>
        public float windowRatio
        {
            get
            {
                return viewportSize.X / viewportSize.Y;
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
