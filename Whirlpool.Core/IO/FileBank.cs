using Whirlpool.Core.Pattern;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.IO
{
    public class FileBank : Singleton<FileBank>
    {
        protected Dictionary<string, Texture> textureBank = new Dictionary<string, Texture>();

        public static Texture GetTexture(string name)
        {
            var instance = GetInstance();
            if (instance.textureBank.ContainsKey(name))
                return instance.textureBank[name];
            else
#if DEBUG
                throw new TextureNotFoundException("Could not find the texture " + name + " (will be replaced with a placeholder in release builds)");
#else
                return instance.textureCache["texNotFound"];
#endif
        }

        public static void LoadTexturesFromFolder(string folder)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".bmp"))
                    AddTexture(file, Texture.FromFile(file));
            }
        }

        public static void AddTexture(string name, Texture texture)
        {
            var instance = GetInstance();

            if (instance.textureBank.ContainsKey(name))
                instance.textureBank.Remove(name);
            instance.textureBank.Add(name, texture);
        }
    }
}
