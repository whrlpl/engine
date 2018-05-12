using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.IO;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.Render
{
    public enum FlipMode
    {
        None,
        FlipX,
        FlipY,
        FlipXAndY
    }

    [NeedsRefactoring]
    public class BaseRenderer : Singleton<BaseRenderer>
    {
        int VAO, VBO, EBO;
        int cubeVAO, cubeVBO, cubeEBO;

        static Material defaultMaterial, spriteMaterial, gradientMaterial, framebufferMaterial;
        protected bool _initialized;

        public Vector2 windowSize;
        public float dpiUpscale = 1.0f;
        public Camera camera;

        protected void _RenderGradient(Vector2 position, Vector2 size)
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            gradientMaterial?.Use();
            if (!_initialized) _Init();

            gradientMaterial.SetVariable("color1", new Color4(0, 0, 0, 0));
            gradientMaterial.SetVariable("color2", new Color4(1, 1, 1, 1));
            gradientMaterial.SetVariable("position", _PixelsToNDC(position));
            gradientMaterial.SetVariable("size", _PixelsToNDCSize(size));
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
        
        protected void _RenderFramebuffer(Vector2 position, Vector2 size, Texture texture)
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            framebufferMaterial?.Use();
            if (!_initialized) _Init();
            texture.Bind();

            framebufferMaterial.SetVariable("flipX", false);
            framebufferMaterial.SetVariable("flipY", false);

            framebufferMaterial.SetVariable("renderedTexture", 0);
            framebufferMaterial.SetVariable("position", _PixelsToNDC(position));
            framebufferMaterial.SetVariable("size", _PixelsToNDCSize(size));
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        protected void _RenderQuad(Vector2 position, Vector2 size, Texture texture, float textureRepetitions, Color4 tint, float rotation, FlipMode flipMode, Material material)
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            spriteMaterial?.Use();
            if (!_initialized) _Init();
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

        protected void _RenderModel(IO.Object obj, Vector3 position, Vector3 size, Vector3 rotation)
        {
            GL.BindVertexArray(obj.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, obj.VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, obj.EBO);

            defaultMaterial?.Use();
            if (!_initialized) _Init();
            Matrix4 model = Matrix4.Identity;
            Matrix4 mvp = model * camera.vp * Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z) * Matrix4.CreateScale(size)/* * Matrix4.CreateTranslation(position)*/;

            defaultMaterial.SetVariable("albedoTexture", 0);
            defaultMaterial.SetVariable("mvp", mvp);
            defaultMaterial.SetVariable("textureRepetitions", 0);
            defaultMaterial.SetVariable("tint", Color4.Red);
            defaultMaterial.SetVariable("time", Time.currentTime);

            GL.DrawElements(BeginMode.Triangles, obj.indices.Count - 1, DrawElementsType.UnsignedInt, 0);
        }

        protected void _Init()
        {
            defaultMaterial = new MaterialBuilder()
                .Build()
                .Attach(new Shader("Shaders\\vert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\frag.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();
            
            spriteMaterial = new MaterialBuilder()
                .Build()
                .Attach(new Shader("Shaders\\spritevert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\spritefrag.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();

            gradientMaterial = new MaterialBuilder()
                .Build()
                .Attach(new Shader("Shaders\\spritevert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\grad.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();

            framebufferMaterial = new MaterialBuilder()
                .Build()
                .Attach(new Shader("Shaders\\spritevert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\fbFrag.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();

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

            PostProcessing.GetInstance().Init(windowSize);

            camera = new Camera();

            _initialized = true;
        }

        protected Vector2 _PixelsToNDC(Vector2 pixels)
        {
            return new Vector2((2 / windowSize.X * dpiUpscale) * -pixels.X + 1, (2 / windowSize.Y * dpiUpscale) * pixels.Y - 1);
        }

        protected Vector2 _PixelsToNDCSize(Vector2 pixels)
        {
            return new Vector2((2 / windowSize.X * dpiUpscale) * pixels.X / 2, (2 / windowSize.Y * dpiUpscale) * pixels.Y / 2);
        }

        public static void RenderFramebuffer(Vector2 position, Vector2 size, Texture texture)
        {
            GetInstance()._RenderFramebuffer(position, size, texture);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, Texture texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetInstance()._RenderQuad(position, size, texture, textureRepetitions, tint, rotation, flipMode, material);
        }

        public static void RenderQuad(Vector2 position, Vector2 size, string texture, Color4 tint, Material material = null, float textureRepetitions = 1, float rotation = 0, FlipMode flipMode = FlipMode.None)
        {
            GetInstance()._RenderQuad(position, size, GetTextureFromString(texture), textureRepetitions, tint, rotation, flipMode, material);
        }

        public static void RenderGradient(Vector2 position, Vector2 size)
        {
            GetInstance()._RenderGradient(position, size);
        }

        protected static Texture GetTextureFromString(string texture)
        {
            return FileBank.GetTexture(texture);
        }

        public static void RenderModel(IO.Object obj, Vector3 position, Vector3 size, Vector3 rotation)
        {
            GetInstance()._RenderModel(obj, position, size, rotation);
        }
    }
}
