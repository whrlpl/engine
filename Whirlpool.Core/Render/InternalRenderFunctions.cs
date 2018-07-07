using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.IO;
using Whirlpool.Core.IO.Assets;
using Whirlpool.Core.Pattern;

namespace Whirlpool.Core.Render
{
    public enum FlipMode
    {
        None,
        FlipX,
        FlipY,
        FlipXAndY
    }

    public partial class Renderer : Singleton<Renderer>
    {
        int VAO, VBO, EBO;

        static Material defaultMaterial, spriteMaterial, framebufferMaterial;
        protected bool _initialized;

        public Vector2 windowSize;
        public float dpiUpscale = 1.0f;
        public Camera camera;

        public Texture blurTextureTest;
                
        protected void _RenderFramebuffer(Vector2 position, Vector2 size, Texture texture)
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            framebufferMaterial?.Use();
            if (!_initialized) _Init();
            texture.Bind();
            //blurTextureTest.Bind();

            framebufferMaterial.SetVariable("flipX", false);
            framebufferMaterial.SetVariable("flipY", false);

            framebufferMaterial.SetVariable("renderedTexture", 0);
            //framebufferMaterial.SetVariable("blurTexture", 1);

            framebufferMaterial.SetVariable("position", _PixelsToNDC(position));
            framebufferMaterial.SetVariable("size", _PixelsToNDCSize(size));
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        protected void _RenderAtlas(Vector2 position, Vector2 size, Vector2 texturePoint, Vector2 textureSize, Texture texture)
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            spriteMaterial?.Use();
            if (!_initialized) _Init();
            texture.Bind();

            Vector2 imageSize = new Vector2(texture.width, texture.height);

            spriteMaterial.SetVariable("albedoTexture", 0);
            spriteMaterial.SetVariable("position", _PixelsToNDC(position));
            spriteMaterial.SetVariable("size", _PixelsToNDCSize(size));
            spriteMaterial.SetVariable("atlas", true);
            spriteMaterial.SetVariable("atlasPoint", _PixelsToNDCImg(texturePoint, imageSize));
            spriteMaterial.SetVariable("atlasSize", _PixelsToNDCImgSize(textureSize, imageSize));
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

        protected void _RenderMesh(Mesh mesh, Vector3 position, Vector3 size, Vector3 rotation)
        {
            GL.BindVertexArray(mesh.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.EBO);

            defaultMaterial?.Use();
            if (!_initialized) _Init();
            Matrix4 model = Matrix4.CreateTranslation(position);
            model.Transpose(); // wtf??? createtranslation gives a row-major matrix for some reason despite opentk being opengl focused...
            model = model * Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(new Vector3(
                    MathHelper.DegreesToRadians(rotation.X), 
                    MathHelper.DegreesToRadians(rotation.Y), 
                    MathHelper.DegreesToRadians(rotation.Z)))) * Matrix4.CreateScale(size);

            defaultMaterial.SetVariable("albedoTexture", 0);
            defaultMaterial.SetVariable("vp", camera.vp);
            defaultMaterial.SetVariable("model", model);
            defaultMaterial.SetVariable("textureRepetitions", 0);
            defaultMaterial.SetVariable("tint", Color4.Red);
            defaultMaterial.SetVariable("mainLightPos", new Vector3(-1.0f, 0.0f, 1.0f));
            defaultMaterial.SetVariable("time", Time.currentTime);

            GL.DrawElements(BeginMode.Triangles, mesh.vertexIndices.Count - 1, DrawElementsType.UnsignedInt, 0);
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
                /* Vertices     Texcoords       Normals (not used)*/
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

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, buffers.Length * sizeof(float), buffers, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            PostProcessing.GetInstance().Init(windowSize);

            blurTextureTest = TextureLoader.LoadAsset("Content\\blurtexturetest.png");
            blurTextureTest.textureUnit = TextureUnit.Texture1;

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

        protected Vector2 _PixelsToNDCImg(Vector2 pixels, Vector2 imgSize)
        {
            return new Vector2((2 / imgSize.X * dpiUpscale) * -pixels.X + 1, (2 / imgSize.Y * dpiUpscale) * pixels.Y - 1);
        }

        protected Vector2 _PixelsToNDCImgSize(Vector2 pixels, Vector2 imgSize)
        {
            return new Vector2((2 / imgSize.X * dpiUpscale) * pixels.X / 2, (2 / imgSize.Y * dpiUpscale) * pixels.Y / 2);
        }
    }
}
