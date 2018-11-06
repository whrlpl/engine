using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.Render
{
    public class Texture3D
    {
        public string name;
        public int glTexture = 0;
        public bool animated = true;
        public int animFrame = -1;
        public int width;
        public int height;

        public TextureWrapMode textureWrapMode = TextureWrapMode.Repeat;
        public TextureMagFilter textureMagFilter = TextureMagFilter.Nearest;
        public TextureMinFilter textureMinFilter = TextureMinFilter.NearestMipmapNearest;
        public TextureUnit textureUnit = TextureUnit.Texture0;
        private byte[] data;

        public static Texture3D FromData(Color4[] data, int width, int height, int depth, bool retainData = false)
        {
            Texture3D temp = new Texture3D()
            {
                name = "textureFromData_" + width + height
            };

            using (var stream = new MemoryStream())
            {
                byte[] rawData;
                GL.GenTextures(1, out temp.glTexture);
                GL.BindTexture(TextureTarget.Texture3D, temp.glTexture);
                rawData = new byte[data.Length * 4];
                for (int i = 0; i < data.Length; ++i)
                {
                    rawData[i * 4 + 2] = (byte)(data[i].R * 255);
                    rawData[i * 4 + 1] = (byte)(data[i].G * 255);
                    rawData[i * 4] = (byte)(data[i].B * 255);
                }
                IntPtr ptr = Marshal.AllocHGlobal(rawData.Length);
                Marshal.Copy(rawData, 0, ptr, rawData.Length);
                GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgb, width, height, depth, 0, PixelFormat.Bgr, PixelType.UnsignedByte, ptr);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture3D);
                temp.width = width;
                temp.height = height;
                if (retainData)
                    temp.data = rawData;
                Marshal.FreeHGlobal(ptr);
            }
            return temp;
        }

        public void Bind()
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture3D, glTexture);

            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)textureMagFilter);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
            GL.TexParameter(TextureTarget.Texture3D, (TextureParameterName)All.TextureMaxAnisotropyExt, 16.0f);
        }

        public void SetData(byte[] data)
        {
            this.data = data;
        }

        public byte[] GetData()
        {
            return data;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
