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
    public class Button : UIComponent
    {
        public Label label;

        public override void Init()
        {
            label = new Label()
            {
                centered = centered,
                position = position,
                text = text,
                font = font
            };

            InputHandler.GetInstance().onMousePressed += (s, e) =>
            {
                var status = InputHandler.GetStatus();
                if (status.mouseButtonLeft)
                {
                    if (new Rectangle(position.X, position.Y, size.X, size.Y).Contains(status.mousePosition))
                    {
                        Logging.Write("Button pressed");
                    }
                }
            };
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, "blank", tint: new Color4(150, 150, 150, 150));
            label.Render();
        }

        public override void Update() { }
    }
}
