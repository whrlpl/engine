using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTKTest.IO;
using OpenTKTest.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Render
{
    public enum FlipMode
    {
        None,
        FlipX,
        FlipY,
        FlipXAndY
    }

    public class BaseRenderer : Singleton<BaseRenderer>
    {
        Texture activeTexture;
        int VAO, VBO, EBO;
        int cubeVAO, cubeVBO, cubeEBO;
        Material defaultMaterial;
        Material spriteMaterial;
        protected bool _initialized;

        public Vector2 windowSize;
        public float dpiUpscale = 1.0f;

        protected void _RenderQuad(Vector2 position, Vector2 size, Texture texture, float textureRepetitions, Color4 tint, float rotation, FlipMode flipMode)
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            spriteMaterial?.Use();
            if (!_initialized)
            {
                _Init();
                _initialized = true;
            }
            texture.Bind();

            spriteMaterial.SetVariable("flipX", false);
            spriteMaterial.SetVariable("flipY", false);
            switch (flipMode)
            {
                case FlipMode.FlipX:
                    spriteMaterial.SetVariable("flipX", true);
                    break;
                case FlipMode.FlipY:
                    spriteMaterial.SetVariable("flipY", true);
                    break;
                case FlipMode.FlipXAndY:
                    spriteMaterial.SetVariable("flipX", true);
                    spriteMaterial.SetVariable("flipY", true);
                    break;
            }
            spriteMaterial.SetVariable("albedoTexture", 0);
            spriteMaterial.SetVariable("textureRepetitions", textureRepetitions);
            spriteMaterial.SetVariable("tint", tint);
            spriteMaterial.SetVariable("position", _PixelsToNDC(position));
            spriteMaterial.SetVariable("size", _PixelsToNDCSize(size));
            spriteMaterial.SetVariable("rotation", rotation);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        protected void _RenderCube(Vector3 position, Vector3 size, Texture texture, float textureRepetitions, Color4 tint)
        {
            GL.BindVertexArray(cubeVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, cubeVBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, cubeEBO);

            defaultMaterial?.Use();
            if (!_initialized)
            {
                _Init();
                _initialized = true;
            }
            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);
            float windowRatio = (float)viewport[2] / viewport[3];
            Matrix4 view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 4.0f), new Vector3(0.0f, 0.0f, -4.0f), new Vector3(0.0f, 1.0f, 0.0f));
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), windowRatio, 0.1f, 100.0f);
            Matrix4 model = Matrix4.Identity;
            Matrix4 mvp = model * view * projection * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 1.0f, 0.0f), Time.currentTime) * Matrix4.CreateScale(size);

            defaultMaterial.SetVariable("albedoTexture", 0);
            defaultMaterial.SetVariable("mvp", mvp);
            defaultMaterial.SetVariable("textureRepetitions", textureRepetitions);
            defaultMaterial.SetVariable("time", Time.currentTime);
            GL.DrawElements(BeginMode.Triangles, 108, DrawElementsType.UnsignedInt, 0);
        }

        protected void _Init()
        {
            defaultMaterial = new Material();
            defaultMaterial.Attach(new Shader("Shaders\\vert.glsl", ShaderType.VertexShader));
            defaultMaterial.Attach(new Shader("Shaders\\frag.glsl", ShaderType.FragmentShader));
            defaultMaterial.Link();

            spriteMaterial = new Material();
            spriteMaterial.Attach(new Shader("Shaders\\spritevert.glsl", ShaderType.VertexShader));
            spriteMaterial.Attach(new Shader("Shaders\\spritefrag.glsl", ShaderType.FragmentShader));
            spriteMaterial.Link();

            GL.GenVertexArrays(1, out cubeVAO);
            GL.GenBuffers(1, out cubeVBO);
            GL.GenBuffers(1, out cubeEBO);
            GL.BindVertexArray(cubeVAO);

            float[] cubeVertices = {
                // front
                -1.0f, -1.0f,  1.0f,    1, 1, //0
                -1.0f,  1.0f,  1.0f,    0, 1, //1
                 1.0f,  1.0f,  1.0f,    1, 1, //2
                 1.0f, -1.0f,  1.0f,    1, 0, //3
                // back
                -1.0f, -1.0f, -1.0f,    0, 1, //4
                -1.0f,  1.0f, -1.0f,    0, 0, //5
                 1.0f,  1.0f, -1.0f,    1, 0, //6
                 1.0f, -1.0f, -1.0f,    1, 1, //7
            };

            uint[] cubeBuffers = {
                // front
                0, 1, 2,
                2, 3, 0,
                // top
                1, 5, 6,
                6, 2, 1,
                // back
                7, 6, 5,
                5, 4, 7,
                // bottom
                4, 0, 3,
                3, 7, 4,
                // left
                4, 5, 1,
                1, 0, 4,
                // right
                3, 2, 6,
                6, 7, 3,
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, cubeVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, cubeVertices.Length * sizeof(float), cubeVertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, cubeEBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, cubeBuffers.Length * sizeof(float), cubeBuffers, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.GenVertexArrays(1, out VAO);
            GL.GenBuffers(1, out VBO);
            GL.GenBuffers(1, out EBO);
            GL.BindVertexArray(VAO);

            float[] vertices =
            {
                /* Vertices     Texcoords */
                -1, 1, 0,       0, 1,
                -1, -1, 0,      0, 0,
                1, -1, 0,       1, 0,
                1, 1, 0,        1, 1,
            };

            uint[] buffers =
            {
                0, 1, 3,
                1, 2, 3
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, buffers.Length * sizeof(float), buffers, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.Enable(EnableCap.Multisample);            
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthClamp);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
        }

        protected Vector2 _PixelsToNDC(Vector2 pixels)
        {
            return new Vector2((2 / windowSize.X * dpiUpscale) * -pixels.X + 1, (2 / windowSize.Y * dpiUpscale) * pixels.Y - 1);
        }
        protected Vector2 _PixelsToNDCSize(Vector2 pixels)
        {
            return new Vector2((2 / windowSize.X * dpiUpscale) * pixels.X / 2, (2 / windowSize.Y * dpiUpscale) * pixels.Y / 2);
        }


        public static void RenderQuad(Vector2 position, Vector2 size, Texture texture, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetInstance()._RenderQuad(position, size, texture, textureRepetitions, Color4.White, rotation, flipMode);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, Texture texture, Color4 tint, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetInstance()._RenderQuad(position, size, texture, textureRepetitions, tint, rotation, flipMode);
        }

        protected static Texture GetTextureFromString(string texture)
        {
            return FileCache.GetTexture(texture).Bind();
        }

        public static void RenderQuad(Vector2 position, Vector2 size, string texture, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetTextureFromString(texture);
            GetInstance()._RenderQuad(position, size, GetTextureFromString(texture), textureRepetitions, Color4.White, rotation, flipMode);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, string texture, Color4 tint, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetTextureFromString(texture);
            GetInstance()._RenderQuad(position, size, GetTextureFromString(texture), textureRepetitions, tint, rotation, flipMode);
        }

        public static void RenderCube(Vector3 position, Vector3 size, string texture, Color4 tint, float textureRepetitions = 1)
        {
            GetInstance()._RenderCube(position, size, GetTextureFromString(texture), textureRepetitions, tint);
        }

        public static void RenderCube(Vector3 position, Vector3 size, string texture, float textureRepetitions = 1)
        {
            GetInstance()._RenderCube(position, size, GetTextureFromString(texture), textureRepetitions, Color4.White);
        }
    }
}
