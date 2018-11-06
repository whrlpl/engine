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
        public Vector3 camNormal = new Vector3(0, 1, 0);
        public Vector3 worldPosition = new Vector3(0, 0, 0);

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
                var basis = BuildBasis();
                var camPos = Vector3.TransformVector(position, basis);
                var lookAtPos_ = Vector3.TransformVector(lookAtPos, basis);
                return Matrix4.LookAt(camPos, lookAtPos_, cameraUp);
                //return Matrix4.LookAt(position, lookAtPos, cameraUp);
            }
        }

        /// <summary>
        /// Projection matrix for camera matrix calculations.
        /// </summary>
        public Matrix4 projection
        {
            get
            {
                return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fieldOfView), windowRatio, 0.1f, 5000.0f);
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


        // Code ported from riperiperi/mkjs 
        public Matrix4 BuildBasis()
        {
            vAngle = FixDir(vAngle);
            hAngle = FixDir(hAngle);
            var forward = new Vector3((float)Math.Sin(vAngle), 0, (float)-Math.Cos(vAngle));
            //var side = new Vector3((float)Math.Cos(vAngle), 0, (float)Math.Sin(vAngle));
            //var forward = new Vector3((float)Math.Sin(vAngle) * (float)Math.Cos(hAngle), (float)-Math.Sin(hAngle), (float)-Math.Cos(vAngle));
            var side = Vector3.Normalize(Vector3.Cross(cameraUp, forward));
            var basis = GramSchmidt(camNormal, side, forward);
            var tmp = basis[0];
            basis[0] = basis[1];
            basis[1] = tmp;
            return new Matrix4(
                basis[0].X, basis[0].Y, basis[0].Z, 0,
                basis[1].X, basis[1].Y, basis[1].Z, 0,
                basis[2].X, basis[2].Y, basis[2].Z, 0,
                0, 0, 0, 1
            );
        }

        // Code ported from riperiperi/mkjs 
        public Vector3[] GramSchmidt(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var u1 = v1;
            var u2 = v2 - Project(u1, v2);
            var u3 = v3 - Project(u1, v3) - Project(u2, v3);
            return new Vector3[] { Vector3.Normalize(u1), Vector3.Normalize(u2), Vector3.Normalize(u3) };
        }

        // Code ported from riperiperi/mkjs 
        public Vector3 Project(Vector3 u, Vector3 v)
        {
            return u * (Vector3.Dot(u, v) / Vector3.Dot(u, u));
        }

        public float FixDir(float dir)
        {
            return PosMod(dir, (float)Math.PI * 2);
        }

        public float PosMod(float i, float n)
        {
            return (i % n + n) % n;
        }
    }
}
