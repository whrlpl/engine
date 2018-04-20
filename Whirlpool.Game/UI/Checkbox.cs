using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Game.UI
{
    class Checkbox : RenderComponent
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 size = new Vector2(64, 64);
        public bool isChecked = false;
        public Texture checkboxTexture;
        public Texture checkboxCheckedTexture;

        public override void Init()
        {
            // nothing?
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, checkboxTexture, Color4.White);
            if (isChecked)
                BaseRenderer.RenderQuad(position, size, checkboxCheckedTexture, Color4.White);
        }

        public override void Update()
        {
            // TODO: input
        }
    }
}
