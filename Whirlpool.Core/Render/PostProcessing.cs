//#define POSTPROCESSING
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Render
{
    public class PostProcessing : Singleton<PostProcessing>
    {
#if POSTPROCESSING
        // int framebuffer, textureBuffer, rbo;
        // Texture textureBufferTexture;
#endif
		public void Init()
        {
#if POSTPROCESSING
            GL.DrawBuffer(DrawBufferMode.None);
#endif
            GL.Enable(EnableCap.Multisample);            
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.DepthClamp);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Fastest);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
#if POSTPROCESSING
            GL.GenFramebuffers(1, out framebuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.GenTextures(1, out textureBuffer);
            GL.BindTexture(TextureTarget.Texture2D, textureBuffer);

            GL.GenRenderbuffers(1, out rbo);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, 1280, 720);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, 1280, 720, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
#endif
        }

        public void Render()
        {

        }
    }
}
