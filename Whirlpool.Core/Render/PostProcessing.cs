#define POSTPROCESSING
#define DEPTHTEXTURE
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;
using OpenTK.Graphics;

namespace Whirlpool.Core.Render
{
    public class PostProcessing : Singleton<PostProcessing>
    {
        int framebuffer;
        Texture textureBufferTexture, depthBufferTexture;

        int width = 0, height = 0;
        int drawWidth = 0, drawHeight = 0;
		public void Init(Vector2 windowSize, int width_ = -1, int height_ = -1)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
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
                name = "fboTexture",
                width = width,
                height = height,
                animated = false
            };
            depthBufferTexture = new Texture()
            {
                name = "depthBufferTexture",
                width = width,
                height = height,
                animated = false,
                textureUnit = TextureUnit.Texture31
            };
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

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Logging.Write("Framebuffer error: " + GL.GetError(), LogStatus.Error);
        }

        public void PreRender()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.DepthClamp);
            GL.DepthRange(0.0f, 100.0f);
            GL.DepthMask(true);
            GL.Viewport(0, 0, width, height);
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
            var scale = (float)BaseGame.Size.Height / GlobalSettings.Default.renderResolutionY;
            Render2D.DrawFramebuffer(new Vector2(0.0f, 0.0f), new Vector2(GlobalSettings.Default.renderResolutionX * scale, GlobalSettings.Default.renderResolutionY * scale), textureBufferTexture);
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
