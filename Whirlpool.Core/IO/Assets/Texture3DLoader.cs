using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Whirlpool.Core.Render.Type;

namespace Whirlpool.Core.IO.Assets
{
    public class Texture3DLoader
    {
        public static Texture3D LoadAsset(string fileName, int width, int height, int depth)
        {
            Texture3D temp = new Texture3D()
            {
                name = fileName
            };
            byte[] rawData;
            using (var sr = new StreamReader(fileName))
            using (var f = Image.FromStream(sr.BaseStream))
            using (var stream = new MemoryStream())
            {
                GL.GenTextures(1, out temp.glTexture);
                GL.BindTexture(TextureTarget.Texture3D, temp.glTexture);
                f.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                rawData = new byte[stream.Length];
                stream.Read(rawData, 0, (int)stream.Length);
                IntPtr ptr = Marshal.AllocHGlobal(rawData.Length);
                Marshal.Copy(rawData, 0, ptr, rawData.Length);
                PixelFormat imageFormat = PixelFormat.Bgra;
                if (f.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb || f.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                    imageFormat = PixelFormat.Bgr;
                GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba, width, height, depth, 0, imageFormat, PixelType.UnsignedByte, ptr);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture3D);
                temp.width = f.Width;
                temp.height = f.Height;
                temp.SetData(rawData);
                Marshal.FreeHGlobal(ptr);
            }
            return temp;
        }
    }
}
