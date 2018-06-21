using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System.Linq;
using Whirlpool.Core.IO;

namespace Whirlpool.Core.UI
{
    public class SlicedSprite : UIComponent
    {
        public string spriteLoc;
        private Texture[] slicedSprites = new Texture[9];
        private Texture originalSprite;

        public override void Init(Screen screen)
        {
            if (initialized) return;
            // gen 9 sprites
            originalSprite = FileBank.GetTexture(spriteLoc);
            var sliceWidth = originalSprite.width / 9;
            var sliceHeight = originalSprite.height / 9;
            var originalData = originalSprite.GetData();
            for (int i = 0; i < 9; ++i)
            {
                Color4[] data = new Color4[sliceWidth * sliceHeight];

                for (int o = 0; o < sliceWidth * sliceHeight; ++o)
                {
                    int offset = sliceWidth * sliceHeight * i;
                    data[o] = new Color4(
                        originalData[offset + o + 3],
                        originalData[offset + o + 2],
                        originalData[offset + o + 1],
                        originalData[offset + o]
                    );
                }
                slicedSprites[i] = Texture.FromData(data.ToArray(), sliceWidth, sliceHeight);
            }
            initialized = true;
        }

        public override void Render()
        {
            Renderer.RenderQuad(position, size, originalSprite, tint);
        }

        public override void Update()
        {
            // Nothing to do here.
        }
    }
}
