using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Render
{
    public class GBuffer
    {
        int gbo; // Gbuffer object.  Yes i made this up
        int posBuf, normalBuf, diffBuf;
        int BindTexture(PixelInternalFormat internalFormat, PixelFormat format, PixelType type, FramebufferAttachment attachment)
        {
            GL.GenTextures(1, out int output);
            GL.BindTexture(TextureTarget.Texture2D, output);
            GL.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, (int)RenderShared.renderResolution.X, (int)RenderShared.renderResolution.Y, 0, format, type, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachment, TextureTarget.Texture2D, output, 0);
            return output;

        }

        public GBuffer()
        {
            GL.GenFramebuffers(1, out gbo);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, gbo);


            posBuf = BindTexture(PixelInternalFormat.Rgb16f, PixelFormat.Rgb, PixelType.Float, FramebufferAttachment.ColorAttachment0);
            normalBuf = BindTexture(PixelInternalFormat.Rgb16f, PixelFormat.Rgb, PixelType.Float, FramebufferAttachment.ColorAttachment1);
            diffBuf = BindTexture(PixelInternalFormat.Rgba, PixelFormat.Rgba, PixelType.UnsignedByte, FramebufferAttachment.ColorAttachment2);
            DrawBuffersEnum attachments = DrawBuffersEnum.ColorAttachment0 | DrawBuffersEnum.ColorAttachment1 | DrawBuffersEnum.ColorAttachment3;
            GL.DrawBuffers(3, ref attachments);

        }


    }
}
