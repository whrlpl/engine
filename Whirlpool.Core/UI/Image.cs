using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.UI
{
    public class Image : UIComponent
    {
        public string spriteLoc;
        private Texture image;

        public override void Init(Screen screen)
        {
            if (initialized) return;
            initialized = true;
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, image, tint);
        }

        public override void Update() { }
    }
}
