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
    public class Checkbox : UIComponent
    {
        public new Vector2 size = new Vector2(64, 64);
        public bool isChecked = false;
        public Texture checkboxTexture;
        public Texture checkboxCheckedTexture;

        public override void Init() { }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, checkboxTexture, Color4.White);
            if (isChecked)
                BaseRenderer.RenderQuad(position, size, checkboxCheckedTexture, Color4.White);
        }

        public override void Update()
        {

        }
    }
}
