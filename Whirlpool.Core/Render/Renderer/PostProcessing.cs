using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using Whirlpool.Core.IO;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Render.Type;

namespace Whirlpool.Core.Render.Renderer
{
    public class PostProcessing : Singleton<PostProcessing>
    {
        public Material frameBufferMaterial = null;
        int framebuffer;
        Texture textureBufferTexture, depthBufferTexture;
        Texture colorTexture;

        Vector2 windowSize;

        int width = 0, height = 0;
        int drawWidth = 0, drawHeight = 0;
		public void Init(Vector2 windowSize_, int width_ = -1, int height_ = -1)
        {
            windowSize = windowSize_;
            drawWidth = (int)windowSize.X;
            drawHeight = (int)windowSize.Y;
            width = width_;
            height = height_;
            if (width_ <= 0)
                width = (int)windowSize.X;
            if (height_ <= 0)
                height = (int)windowSize.Y;
            textureBufferTexture = new Texture()
            {
                fileName = "fboTexture",
                width = width,
                height = height,
                animated = false
            };
            depthBufferTexture = new Texture()
            {
                fileName = "depthBufferTexture",
                width = width,
                height = height,
                animated = false,
                textureUnit = TextureUnit.Texture31
            };

            colorTexture = IO.Content.GetTexture("clut.png");
            colorTexture.textureMinFilter = TextureMinFilter.Nearest;
            colorTexture.textureMagFilter = TextureMagFilter.Nearest;
            colorTexture.textureUnit = TextureUnit.Texture30;

            GL.GenFramebuffers(1, out framebuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

            GL.ActiveTexture(TextureUnit.Texture0);

            GL.GenTextures(1, out textureBufferTexture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, textureBufferTexture.glTexture);
            textureBufferTexture.textureMagFilter = TextureMagFilter.Nearest;
            textureBufferTexture.textureMinFilter = TextureMinFilter.Nearest;
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);


            GL.ActiveTexture(TextureUnit.Texture31);
            GL.GenTextures(1, out depthBufferTexture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, depthBufferTexture.glTexture);
            depthBufferTexture.textureMagFilter = TextureMagFilter.Nearest;
            depthBufferTexture.textureMinFilter = TextureMinFilter.Nearest;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareFunc, (int)DepthFunction.Lequal);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, width, height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureBufferTexture.glTexture, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, depthBufferTexture.glTexture, 0);

            frameBufferMaterial = new MaterialBuilder()
                .Build()
                .SetName("Default Sprite Material")
                .Attach(new Shader("Shaders\\2D\\vert.glsl", ShaderType.VertexShader))
                .Attach(new Shader("Shaders\\2D\\fbFrag.glsl", ShaderType.FragmentShader))
                .Link()
                .GetMaterial();

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Logging.Write("Framebuffer error: " + GL.GetError(), LogStatus.Error);
        }

        public void PreRender()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.Texture3DExt);
            GL.Enable(EnableCap.Blend);
            //GL.Enable(EnableCap.CullFace);
            //GL.FrontFace(FrontFaceDirection.Cw);
            //GL.CullFace(CullFaceMode.Front);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Enable(EnableCap.DepthClamp);
            GL.DepthRange(0.0f, 100.0f);
            GL.DepthMask(true);
            GL.Viewport(0, 0, width, height);
        }

        public void Prepare2D()
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        public void PostRender()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.DepthTest);
            GL.Viewport(0, 0, drawWidth, drawHeight);
            if (textureBufferTexture == null)
            {
                Console.WriteLine("???");
                return;
            }
            depthBufferTexture.Bind();
            colorTexture.Bind();
            var scale = (float)BaseGame.Size.Height / GlobalSettings.Default.renderResolutionY;
            if (GlobalSettings.Default.renderResolutionX <= 0)
                width = (int)windowSize.X;
            if (GlobalSettings.Default.renderResolutionY <= 0)
                height = (int)windowSize.Y;
            Renderer2D.DrawFramebuffer(new Vector2(0.0f, 0.0f), new Vector2(GlobalSettings.Default.renderResolutionX * scale, GlobalSettings.Default.renderResolutionY * scale), textureBufferTexture, frameBufferMaterial);
        }

        public void Resize(Size windowSize)
        {
            if (textureBufferTexture == null) return;
            width = windowSize.Width;
            height = windowSize.Height;
            GL.BindTexture(TextureTarget.Texture2D, depthBufferTexture.glTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.Byte, IntPtr.Zero);
        }
    }
}
