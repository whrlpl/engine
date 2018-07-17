using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Render
{
    public class Render2D
    {
        public static Material defaultSpriteMaterial;
        private static int QuadVAO, QuadVBO, QuadEBO;

        public static Vector2 renderResolution;

        public enum FlipMode : byte
        {
            X = 0b0001,
            Y = 0b0010,
            None = 0b0000
        }


        public static void Init()
        {
            renderResolution = new Vector2(320, 240);

            GL.GenVertexArrays(1, out QuadVAO);
            GL.GenBuffers(1, out QuadVBO);
            GL.GenBuffers(1, out QuadEBO);
            GL.BindVertexArray(QuadVAO);

            float[] vertices =
            {
                /* Verts        Texcoords       Normals*/
                -1, 1, 0,       0, 1,           0, 0, 0,
                -1, -1, 0,      0, 0,           0, 0, 0,
                1, -1, 0,       1, 0,           0, 0, 0,
                1, 1, 0,        1, 1,           0, 0, 0
            };

            uint[] buffers =
            {
                0, 1, 3,
                1, 2, 3
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, QuadVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, QuadEBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, buffers.Length * sizeof(float), buffers, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            defaultSpriteMaterial = new MaterialBuilder()
                .Build()
                .Attach(new Shader("Shaders\\2D\\vert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\2D\\frag.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();
        }

        public static void DrawQuad(Vector2 position, Vector2 scale, Texture texture = null, Material material = null, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GL.BindVertexArray(QuadVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, QuadVBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, QuadEBO);
            if (material == null) material = defaultSpriteMaterial;

            material.Use();
            texture?.Bind();

            material.SetVariables(new Dictionary<string, Type.Any>(){
                { "FlipX", false },
                { "FlipY", false },
                { "AlbedoTexture", 0 },
                { "Position", PixelsToNDC(position) },
                { "Scale", PixelsToNDCScale(scale) }
            });

            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
        
        public static void DrawFramebuffer(Vector2 position, Vector2 scale, Texture texture = null)
        {
            GL.BindVertexArray(QuadVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, QuadVBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, QuadEBO);
            Material material = defaultSpriteMaterial;

            material.Use();
            texture?.Bind();

            material.SetVariables(new Dictionary<string, Type.Any>(){
                { "FlipX", false },
                { "FlipY", false },
                { "AlbedoTexture", 0 },
                { "Position", position + new Vector2(PixelsToNDCScaleFBO(scale).X, -PixelsToNDCScaleFBO(scale).Y)},
                { "Scale", PixelsToNDCScaleFBO(scale) }
            });

            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
        protected static Vector2 PixelsToNDCFBO(Vector2 pixels)
        {
            return new Vector2((2.0f / BaseGame.Size.Width) * -pixels.X + 1, (2.0f / BaseGame.Size.Height) * pixels.Y - 1.0f);
        }

        protected static Vector2 PixelsToNDCScaleFBO(Vector2 pixels)
        {
            return new Vector2((2.0f / BaseGame.Size.Width) * pixels.X / 2.0f, (2.0f / BaseGame.Size.Height) * pixels.Y / 2.0f);
        }

        protected static Vector2 PixelsToNDC(Vector2 pixels)
        {
            return new Vector2((2 / renderResolution.X) * -pixels.X + 1, (2 / renderResolution.Y) * pixels.Y - 1);
        }

        protected static Vector2 PixelsToNDCScale(Vector2 pixels)
        {
            return new Vector2((2 / renderResolution.X) * pixels.X / 2, (2 / renderResolution.Y) * pixels.Y / 2);
        }

        protected static Vector2 PixelsToNDCImg(Vector2 pixels, Vector2 imgSize)
        {
            return new Vector2((2 / imgSize.X) * -pixels.X + 1, (2 / imgSize.Y) * pixels.Y - 1);
        }

        protected static Vector2 PixelsToNDCImgSize(Vector2 pixels, Vector2 imgSize)
        {
            return new Vector2((2 / imgSize.X) * pixels.X / 2, (2 / imgSize.Y) * pixels.Y / 2);
        }
    }
}
