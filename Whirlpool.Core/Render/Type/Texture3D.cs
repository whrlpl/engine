using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Whirlpool.Core.IO;
using Whirlpool.Shared;

namespace Whirlpool.Core.Render.Type
{
    public class Texture3D : IAsset
    {
        public string name;
        public int glTexture = 0;
        public bool animated = true;
        public int animFrame = -1;
        public int width;
        public int height;
        public int depth;

        public TextureWrapMode textureWrapMode = TextureWrapMode.Repeat;
        public TextureMagFilter textureMagFilter = TextureMagFilter.Nearest;
        public TextureMinFilter textureMinFilter = TextureMinFilter.NearestMipmapNearest;
        public TextureUnit textureUnit = TextureUnit.Texture0;
        private byte[] data;

        public Texture3D() { }

        public string fileName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        byte[] IAsset.data { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void LoadDataAsset(string fileName, byte[] fileData)
        {
            width = 512;
            height = 512;
            depth = 16;
            name = "textureFromData_" + width + height;
            using (var stream = new MemoryStream())
            {
                GL.GenTextures(1, out glTexture);
                GL.BindTexture(TextureTarget.Texture3D, glTexture);
                IntPtr ptr = Marshal.AllocHGlobal(fileData.Length);
                PixelFormat imageFormat = PixelFormat.Bgra;
                GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.Rgba, width, height, depth, 0, imageFormat, PixelType.UnsignedByte, ptr);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture3D);
                Marshal.FreeHGlobal(ptr);
            }
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

        public void LoadAsset(PackageFile file)
        {
            LoadDataAsset(file.fileName, file.fileData);
        }
    }
}
