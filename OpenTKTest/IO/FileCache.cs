using OpenTKTest.Pattern;
using OpenTKTest.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.IO
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

        public static void AddTexture(string name, Texture texture)
        {
            var instance = GetInstance();

            if (instance.textureCache.ContainsKey(name))
#if DEBUG
                throw new TextureAlreadyExistsException("The texture " + name + " has already been added (and will be replaced in release builds)");
#else
                instance.textureCache.Remove(name);
#endif
            instance.textureCache.Add(name, texture);
        }
    }
}
