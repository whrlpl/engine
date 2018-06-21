using System.Collections.Generic;
using System.IO;
using Whirlpool.Core.IO.Assets;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Render;
using Whirlpool.Core.UI;

namespace Whirlpool.Core.IO
{
    public class FileBank : Singleton<FileBank>
    {
        /// <summary>
        /// Textures loaded into the game, with the key being the texture path relative to the game executable.
        /// </summary>
        protected Dictionary<string, Texture> textureBank = new Dictionary<string, Texture>();

        /// <summary>
        /// Fonts loaded into the game, with the key being the font path relative to the game executable.
        /// </summary>
        protected Dictionary<string, Font> fontBank = new Dictionary<string, Font>();

        /// <summary>
        /// Get a texture from the texture bank by its file name.
        /// </summary>
        /// <param name="name">The texture's file name</param>
        /// <returns>The texture</returns>
        public static Texture GetTexture(string name)
        {
            var instance = GetInstance();
            if (name != null && instance.textureBank.ContainsKey(name))
                return instance.textureBank[name];
            else
#if DEBUG
                throw new TextureNotFoundException("Could not find the texture " + name + " (will be replaced with a placeholder in release builds)");
#else
                return instance.textureCache["texNotFound"];
#endif
        }

        /// <summary>
        /// Load all the textures in a directory recursively.
        /// </summary>
        /// <param name="folder">The directory to load from</param>
        public static void LoadTexturesFromFolder(string folder)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".bmp"))
                    AddTexture(file, TextureLoader.LoadAsset(file));
            }
        }

        /// <summary>
        /// Add a texture to the file bank.
        /// </summary>
        /// <param name="name">The path / name of the texture</param>
        /// <param name="texture">The texture</param>
        public static void AddTexture(string name, Texture texture)
        {
            var instance = GetInstance();

            if (instance.textureBank.ContainsKey(name))
                instance.textureBank.Remove(name);
            instance.textureBank.Add(name, texture);
        }
    }
}
