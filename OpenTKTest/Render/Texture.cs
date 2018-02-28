using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace OpenTKTest.Render
{
    public class Texture
    {
        public string name;
        public int glTexture = 0;
        public bool animated = true;
        public int animFrame = -1;

        public static Texture FromData(Color4[] data, int width, int height)
        {
            Texture temp = new Texture()
            {
                name = string.Empty
            };

            byte[] rawData;
            using (var stream = new MemoryStream())
            {
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
                Marshal.FreeHGlobal(ptr);
            }
            return temp;
        }

        public static Texture FromFile(string fileName)
        {
            Texture temp = new Texture()
            {
                name = fileName
            };
            byte[] rawData;
            using (var sr = new StreamReader(fileName))
                using (var f = Image.FromStream(sr.BaseStream))
                    using (var stream = new MemoryStream())
                    {
                        GL.GenTextures(1, out temp.glTexture);
                        GL.BindTexture(TextureTarget.Texture2D, temp.glTexture);
                        f.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        rawData = new byte[stream.Length];
                        stream.Read(rawData, 0, (int)stream.Length);
                        IntPtr ptr = Marshal.AllocHGlobal(rawData.Length);
                        Marshal.Copy(rawData, 0, ptr, rawData.Length);
                        PixelFormat imageFormat = PixelFormat.Bgra;
                        if (f.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb || f.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                            imageFormat = PixelFormat.Bgr;
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, f.Width, f.Height - 1, 0, imageFormat, PixelType.UnsignedByte, ptr);
                        Marshal.FreeHGlobal(ptr);
                    }
            return temp;
        }

        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.glTexture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        }
    }
}
