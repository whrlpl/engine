using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.IO.Assets
{
    public class TextureLoader
    {
        public static Texture LoadAsset(string fileName)
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
                temp.width = f.Width;
                temp.height = f.Height;
                temp.SetData(rawData);
                Marshal.FreeHGlobal(ptr);
            }
            return temp;
        }
    }
}
