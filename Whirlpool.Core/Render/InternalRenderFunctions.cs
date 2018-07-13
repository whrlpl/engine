using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
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

        public static Material defaultMaterial, spriteMaterial, framebufferMaterial;
        protected bool _initialized;

        public Vector2 windowSize;
        public float dpiUpscale = 1.0f;
        public Camera camera;

        public Texture blurTextureTest;

        public Vector2 renderResolution = new Vector2(-1, -1);
                
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

            //framebufferMaterial.SetVariable("position", new Vector2(1.0f, -1.0f));
            //framebufferMaterial.SetVariable("size", new Vector2(1, 1));
            framebufferMaterial.SetVariable("position", position);
            framebufferMaterial.SetVariable("size", size);
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

        protected void _RenderQuad(Vector2 position, Vector2 size, Texture texture, float textureRepetitions, Color4 tint, float rotation, FlipMode flipMode, Material material, float opacity)
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
            // there are only 32 levels of transparency supported by a ps1 console
            spriteMaterial.SetVariable("opacity", (float)(Math.Round(opacity * 32) / 32));
            spriteMaterial.SetVariable("position", _PixelsToNDC(position));
            spriteMaterial.SetVariable("size", _PixelsToNDCSize(size));
            spriteMaterial.SetVariable("rotation", rotation);
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        protected void _RenderMesh(Mesh mesh, Vector3 position, Vector3 size, Quaternion rotation, Texture texture, Material material)
        {
            GL.BindVertexArray(mesh.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.EBO);
            if (material == null) material = defaultMaterial;

            material?.Use();
            if (!_initialized) _Init();
            Matrix4 model = Matrix4.CreateTranslation(position);
            model.Transpose();
            model = model * Matrix4.CreateScale(size) * Matrix4.CreateFromQuaternion(rotation);
            //Matrix4 model = Matrix4.CreateScale(size);
            //model = model * (Matrix4.CreateRotationX(rotation.X) + Matrix4.CreateRotationX(rotation.Y) + Matrix4.CreateRotationX(rotation.Z)) * Matrix4.CreateScale(size);
            //model = Matrix4.Identity;
            //model = Matrix4.CreateTranslation(position);
            texture?.Bind();

            model.Transpose();
            Matrix4 mvp = model * camera.view * camera.projection;

            material.SetVariable("albedoTexture", 0);
            material.SetVariable("depthTexture", 2);
            material.SetVariable("mvp", mvp);
            material.SetVariable("model", model);
            material.SetVariable("vp", camera.view * camera.projection);
            material.SetVariable("textureRepetitions", 0);
            material.SetVariable("tint", Color4.White);
            material.SetVariable("mainLightPos", new Vector3(4, 3, 3));
            material.SetVariable("mainLightTint", Color4.White);
            material.SetVariable("time", Time.currentTime);
            material.SetVariable("position", position);

            GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.vertexIndices.Count);

            //GL.DrawElements(BeginMode.Triangles, mesh.vertexIndices.Count - 1, DrawElementsType.UnsignedInt, 0);
        }

        protected void _Init()
        {
            PostProcessing.GetInstance().Init(windowSize, (int)renderResolution.X, (int)renderResolution.Y);

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

            blurTextureTest = TextureLoader.LoadAsset("Content\\blurtexturetest.png");
            blurTextureTest.textureUnit = TextureUnit.Texture1;

            camera = new Camera();
            camera.viewportSize = renderResolution;

            _initialized = true;
        }

        protected Vector2 _PixelsToNDCFramebuffer(Vector2 pixels)
        {
            return new Vector2((2 / windowSize.X * dpiUpscale) * -pixels.X + 1, (2 / windowSize.Y * dpiUpscale) * pixels.Y - 1);
        }

        protected Vector2 _PixelsToNDCSizeFramebuffer(Vector2 pixels)
        {
            return new Vector2((2 / windowSize.X * dpiUpscale) * pixels.X / 2, (2 / windowSize.Y * dpiUpscale) * pixels.Y / 2);
        }

        protected Vector2 _PixelsToNDC(Vector2 pixels)
        {
            return new Vector2((2 / renderResolution.X * dpiUpscale) * -pixels.X + 1, (2 / renderResolution.Y * dpiUpscale) * pixels.Y - 1);
        }

        protected Vector2 _PixelsToNDCSize(Vector2 pixels)
        {
            return new Vector2((2 / renderResolution.X * dpiUpscale) * pixels.X / 2, (2 / renderResolution.Y * dpiUpscale) * pixels.Y / 2);
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
