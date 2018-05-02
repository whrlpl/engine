using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.UI
{
    public class SlicedSprite : UIComponent
    {
        public string spriteLoc;
        private Texture[] slicedSprites = new Texture[9];
        private Texture originalSprite;

        public override void Init()
        {
            // gen 9 sprites
            originalSprite = Texture.FromFile(spriteLoc, true);
            var sliceWidth = originalSprite.width / 9;
            var sliceHeight = originalSprite.height / 9;
            var originalData = originalSprite.getData();
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
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, originalSprite, Color4.White);
        }

        public override void Update()
        {
            // Nothing to do here.
        }
    }
}
