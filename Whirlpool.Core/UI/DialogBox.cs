using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;


namespace Whirlpool.Core.UI
{
    public class DialogBox : UIComponent
    {
        public Label label;

        public override void Init(Screen screen)
        {
            label = new Label()
            {
                m_position = position,
                text = text,
                font = font
            };
            initialized = true;
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, "blank", tint: tint);
            label.Render();
        }

        public override void Update()
        { }
    }
}
