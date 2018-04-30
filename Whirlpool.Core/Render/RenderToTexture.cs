#define POSTPROCESSING
using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.Render
{
    [NeedsRefactoring]
    public class RenderToTexture
    {
        int framebuffer;
        public Texture texture;
        int width = 0, height = 0;

        int oldFramebuffer;

        public RenderToTexture(Vector2 size)
        {
            width = (int)size.X;
            height = (int)size.Y;

            texture = new Texture()
            {
                name = "renderTexture",
                width = width,
                height = height,
                animated = false
            };
            GL.GenFramebuffers(1, out framebuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.GenTextures(1, out texture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, texture.glTexture);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.Byte, IntPtr.Zero);

            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, texture.glTexture, 0);
            DrawBuffersEnum[] drawBuffers = new[] { DrawBuffersEnum.ColorAttachment0 };
            GL.DrawBuffers(1, drawBuffers);
        }

        public void Attach()
        {
            GL.GetInteger(GetPName.DrawFramebufferBinding, out oldFramebuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.Viewport(0, 0, width, height);
        }

        public void Detach()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, width, height);
        }
    }
}
