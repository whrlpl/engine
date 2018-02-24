using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKTest.Pattern;
using OpenTKTest.IO;
using OpenTKTest.Render;

namespace OpenTKTest.Game
{
    public class Blocks : Singleton<Blocks>
    {
        protected Dictionary<uint, Block> blocks = new Dictionary<uint, Block>();

        public static Block GetBlock(uint id)
        {
            var instance = GetInstance();
            if (instance.blocks.ContainsKey(id))
                return instance.blocks[id];
            return new Block()
            {
                name = "?",
                texture = ""
            };
        }

        public static void LoadBlocksFromFolder(string folder)
        {
            var instance = GetInstance();
            uint currentBlockId = (uint)instance.blocks.Count;
            foreach (string file in Directory.GetFiles(folder))
            {
                if (file.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Add block
                    var blockTexture = "block_" + currentBlockId;
                    FileCache.AddTexture(blockTexture, Texture.FromFile(file));
                    instance.blocks.Add(currentBlockId, new Block()
                    {
                        texture = blockTexture,
                        name = "??"
                    });
                    ++currentBlockId;
                }
            }
        }
    }
}
