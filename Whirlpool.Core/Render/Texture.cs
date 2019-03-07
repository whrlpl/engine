using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;
using Whirlpool.Shared;

namespace Whirlpool.Core.Render
{
    public class Texture : IAsset
    {
        public string fileName;
        string IAsset.fileName { get { return fileName; } set { fileName = value; } }
        public int glTexture = 0;
        public bool animated = true;
        public int animFrame = -1;
        public int width;
        public int height;

        public TextureWrapMode textureWrapMode = TextureWrapMode.Repeat;
        public TextureMagFilter textureMagFilter = TextureMagFilter.Nearest;
        public TextureMinFilter textureMinFilter = TextureMinFilter.NearestMipmapNearest;
        public TextureUnit textureUnit = TextureUnit.Texture0;
        byte[] data;
        byte[] IAsset.data { get => data; set => data = value; }

        public static Texture FromData(Color4[] data, int width, int height, bool retainData = false)
        {
            Texture temp = new Texture()
            {
                fileName = "textureFromData_" + width + height
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
                //if (retainData)
                //    temp.data = rawData;
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
            GL.TexParameter(TextureTarget.Texture2D, (TextureParameterName)All.TextureMaxAnisotropyExt, 16.0f);
        }

        //public void SetData(byte[] data)
        //{
        //    this.data = data;
        //}

        //public byte[] GetData()
        //{
        //    return data;
        //}

        public override string ToString()
        {
            return this.fileName;
        }

        public void LoadDataAsset(string fileName, byte[] data)
        {
            this.fileName = fileName;
            byte[] rawData;

            using (var stream = new MemoryStream())
            {
                using (var fileDataStream = new MemoryStream(data))
                {
                    using (var f = Image.FromStream(fileDataStream))
                    {
                        GL.GenTextures(1, out glTexture);
                        GL.BindTexture(TextureTarget.Texture2D, glTexture);
                        f.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        PixelFormat imageFormat = PixelFormat.Bgra;
                        if (f.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb || f.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
                            imageFormat = PixelFormat.Bgr;

                        if (imageFormat == PixelFormat.Bgra)
                        {
                            rawData = new byte[stream.Length + (f.Width * 4)];
                            stream.Read(rawData, (f.Width * 4), (int)stream.Length);
                        }
                        else
                        {
                            rawData = new byte[stream.Length + (f.Width * 3)];
                            stream.Read(rawData, (f.Width * 3), (int)stream.Length);
                        }

                        IntPtr ptr = Marshal.AllocHGlobal(rawData.Length);
                        Marshal.Copy(rawData, 0, ptr, rawData.Length);

                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, f.Width, f.Height, 0, imageFormat, PixelType.UnsignedByte, ptr);
                        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                        width = f.Width;
                        height = f.Height;
                        //SetData(rawData);
                        Marshal.FreeHGlobal(ptr);
                    }
                }
            }
        }

        public void LoadAsset(PackageFile file)
        {
            LoadDataAsset(file.fileName, file.fileData);
        }
    }
}
