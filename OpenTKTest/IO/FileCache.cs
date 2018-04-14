using OpenTKTest.Core.Pattern;
using OpenTKTest.Core.Render;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Core.IO
{
    public class FileCache : Singleton<FileCache>
    {
        protected Dictionary<string, Texture> textureCache = new Dictionary<string, Texture>();

        public static Texture GetTexture(string name)
        {
            var instance = GetInstance();
            if (instance.textureCache.ContainsKey(name))
                return instance.textureCache[name];
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

            if (instance.textureCache.ContainsKey(name))
                instance.textureCache.Remove(name);
            instance.textureCache.Add(name, texture);
        }
    }
}
