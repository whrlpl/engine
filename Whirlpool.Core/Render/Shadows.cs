using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace Whirlpool.Core.Render
{
    public class Shadows
    {
        public Material frameBufferMaterial = null;
        int framebuffer;
        Texture depthBufferTexture;

        const int width = 2048, height = 2048;
		public void Init()
        {
            depthBufferTexture = new Texture()
            {
                name = "shadowMapTexture",
                width = width,
                height = height,
                animated = false,
                textureUnit = TextureUnit.Texture0
            };
            GL.GenFramebuffers(1, out framebuffer);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);


            GL.ActiveTexture(TextureUnit.Texture0);
            GL.GenTextures(1, out depthBufferTexture.glTexture);
            GL.BindTexture(TextureTarget.Texture2D, depthBufferTexture.glTexture);
            depthBufferTexture.textureMagFilter = TextureMagFilter.Nearest;
            depthBufferTexture.textureMinFilter = TextureMinFilter.Nearest;
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent16, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthBufferTexture.glTexture, 0);

            GL.DrawBuffer(DrawBufferMode.None);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                Logging.Write("Framebuffer error: " + GL.GetError(), LogStatus.Error);
        }

        public void PrepShadowMap()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthClamp);
            GL.DepthRange(0.0f, 100.0f);
            GL.DepthMask(true);
            GL.Viewport(0, 0, width, height);
        }

        public void DrawShadowMap()
        {
            Renderer2D.DrawQuad(new Vector2(0.0f, 0.0f), new Vector2(100, 100), depthBufferTexture);
        }


        public void FinishShadowMap()
        {
            Renderer2D.DrawQuad(new Vector2(0.0f, 0.0f), new Vector2(100, 100), depthBufferTexture);
        }
    }
}
