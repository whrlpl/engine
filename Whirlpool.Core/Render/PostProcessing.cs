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
            //GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            //GL.CullFace(CullFaceMode.Front);
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
                animated = false,
                textureUnit = TextureUnit.Texture2
            };
            GL.GenFramebuffers(1, out framebuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

            GL.ActiveTexture(TextureUnit.Texture0);

            GL.GenTextures(1, out textureBufferTexture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, textureBufferTexture.glTexture);
            textureBufferTexture.textureMagFilter = TextureMagFilter.Nearest;
            textureBufferTexture.textureMinFilter = TextureMinFilter.Nearest;
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);


#if DEPTHTEXTURE
            GL.GenTextures(1, out depthBufferTexture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, depthBufferTexture.glTexture);
            depthBufferTexture.textureMagFilter = TextureMagFilter.Nearest;
            depthBufferTexture.textureMinFilter = TextureMinFilter.Nearest;
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureCompareFunc, (int)DepthFunction.Lequal);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, width, height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
#else
            GL.GenRenderbuffers(1, out depthbuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, depthbuffer);
#endif
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureBufferTexture.glTexture, 0);
#if DEPTHTEXTURE
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, depthBufferTexture.glTexture, 0);
#endif

#if !DEPTHTEXTURE
            DrawBuffersEnum[] drawBuffers = new[] { DrawBuffersEnum.ColorAttachment0 };
            GL.DrawBuffers(1, drawBuffers);
#endif

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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            //GL.DepthRange(0.5f, 1.0f);
            //GL.ClearDepth(0);
            //if (Time.GetSeconds() % 2 == 0)
            //    GL.Enable(EnableCap.DepthClamp);
            //else
            //    GL.Disable(EnableCap.DepthClamp);
            ////GL.DepthFunc(DepthFunction.Less);


#if POSTPROCESSING
            GL.Viewport(0, 0, width, height);
            //GL.ClearDepth(-1);
#endif
        }

        public void PostRender()
        {
#if POSTPROCESSING
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.ClearColor(Color4.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.DepthTest);
            GL.Viewport(0, 0, drawWidth, drawHeight);
            if (textureBufferTexture == null)
            {
                Console.WriteLine("???");
                return;
            }
            depthBufferTexture.Bind();
            Renderer.framebufferMaterial.SetVariable("fogStrength", 1.0f);
            Renderer.RenderFramebuffer(new Vector2(1.0f, -1.0f), new Vector2(1.0f, 1.0f), textureBufferTexture);
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
