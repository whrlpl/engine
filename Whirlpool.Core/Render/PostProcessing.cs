#define POSTPROCESSING
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
    [NeedsRefactoring]
    public class PostProcessing : Singleton<PostProcessing>
    {
#if POSTPROCESSING
        int framebuffer, depthbuffer;
        Texture textureBufferTexture, depthBufferTexture;

        int width = 0, height = 0;
        int drawWidth = 0, drawHeight = 0;
#endif
		public void Init(Vector2 windowSize, int width_ = -1, int height_ = -1)
        {
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthClamp);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearDepth(1);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
#if POSTPROCESSING
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
                animated = false
            };

            GL.ActiveTexture(TextureUnit.Texture0);

            GL.GenTextures(1, out textureBufferTexture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, textureBufferTexture.glTexture);
            textureBufferTexture.textureMagFilter = TextureMagFilter.Nearest;
            textureBufferTexture.textureMinFilter = TextureMinFilter.Nearest;
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);


            GL.GenTextures(1, out depthBufferTexture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, depthBufferTexture.glTexture);
            depthBufferTexture.textureMagFilter = TextureMagFilter.Nearest;
            depthBufferTexture.textureMinFilter = TextureMinFilter.Nearest;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareFunc, (int)DepthFunction.Lequal);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.GenFramebuffers(1, out framebuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureBufferTexture.glTexture, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthBufferTexture.glTexture, 0);
            //DrawBuffersEnum[] drawBuffers = new[] { DrawBuffersEnum.ColorAttachment0 };
            //GL.DrawBuffers(1, drawBuffers);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Logging.Write("Framebuffer error: " + GL.GetError(), LogStatus.Error);
#endif
        }

        public void PreRender()
        {
#if POSTPROCESSING
            //GL.Enable(EnableCap.Multisample);     
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
#endif
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
#if POSTPROCESSING
            //GL.ClearDepth(-1);
            GL.Viewport(0, 0, width, height);
#endif
        }

        public void PostRender()
        {
#if POSTPROCESSING
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, drawWidth, drawHeight);
            if (textureBufferTexture == null)
            {
                Console.WriteLine("???");
                return;
            }
            Renderer.RenderFramebuffer(new Vector2(1.0f, -1.0f), new Vector2(0.5f, 0.5f), depthBufferTexture);
            Renderer.RenderFramebuffer(new Vector2(0.0f, 0.0f), new Vector2(0.5f, 0.5f), textureBufferTexture);
            //Renderer.RenderFramebuffer(new Vector2(drawWidth / 2, drawHeight / 2), new Vector2(drawWidth / 2, drawHeight / 2), textureBufferTexture);
#endif
        }

        public void Resize(Size windowSize)
        {
#if POSTPROCESSING
            if (textureBufferTexture == null) return;
            width = windowSize.Width;
            height = windowSize.Height;
            GL.BindTexture(TextureTarget.Texture2D, depthBufferTexture.glTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.Byte, IntPtr.Zero);
#endif
        }
    }
}
