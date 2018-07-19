using System;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.Render
{
    public class Texture
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

        public static Texture FromData(Color4[] data, int width, int height, bool retainData = false)
        {
            Texture temp = new Texture()
            {
                name = "textureFromData_" + width + height
            };

            using (var stream = new MemoryStream())
            {
                byte[] rawData;
                GL.GenTextures(1, out temp.glTexture);
                GL.BindTexture(TextureTarget.Texture2D, temp.glTexture);
                rawData = new byte[data.Length * 4];
                for (int i = 0; i < data.Length; ++i)
                {
                    rawData[i * 4] = (byte)(data[i].B * 255);
                    rawData[i * 4 + 1] = (byte)(data[i].G * 255);
                    rawData[i * 4 + 2] = (byte)(data[i].R * 255);
                    rawData[i * 4 + 3] = (byte)(data[i].A * 255);
                }
                IntPtr ptr = Marshal.AllocHGlobal(rawData.Length);
                Marshal.Copy(rawData, 0, ptr, rawData.Length);
                PixelFormat imageFormat = PixelFormat.Bgra;
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, imageFormat, PixelType.UnsignedByte, ptr);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
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
            GL.BindTexture(TextureTarget.Texture2D, glTexture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)textureWrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureMagFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
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
