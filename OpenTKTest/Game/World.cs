using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKTest.Render;

namespace OpenTKTest.Game
{
    public class World
    {
        public Dictionary<Vector3, Block> blockPositions = new Dictionary<Vector3, Block>();
        private const float scale = 0.5f;
        public void Render()
        {
            foreach (KeyValuePair<Vector3, Block> blockPos in blockPositions)
            {
                BaseRenderer.RenderCube(blockPos.Key * scale, new Vector3(scale), blockPos.Value.texture);
            }
        }

        public void Init()
        {
            //for (int x = -10; x < 10; ++x)
            //for (int y = -10; y < 10; ++y)
            //    blockPositions.Add(new Vector3(x, y, -4.0f), Blocks.GetBlock(0x01));
            blockPositions.Add(new Vector3(0, 0, -4.0f), Blocks.GetBlock(0x03));
            blockPositions.Add(new Vector3(-2.0f, -2.0f, -4.0f), Blocks.GetBlock(0x01));
            //blockPositions.Add(new Vector2(0, 0), Blocks.GetBlock(0x00));
        }
    }
}
