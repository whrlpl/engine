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
using Whirlpool.Core.IO.Events;

namespace Whirlpool.Core.UI
{
    public class Button : UIComponent
    {
        public Label label;
        public Color4 backgroundTint = Color4.White;

        public override Vector2 CalculateCenterPos(Vector2 point)
        {
            if (centered)
            {
                return new Vector2(point.X, point.Y - font.baseCharHeight) - (new Vector2(Math.Max(size.X, font.GetStringSize(label.text).X), font.baseCharHeight * 3) / 2);
            }
            return point;
        }

        public override void Init(Screen screen)
        {
            var labelPos = (centered) ? position + new Vector2(8, font.baseCharHeight * 2) : position;
            label = new Label()
            {
                m_position = labelPos,
                text = text,
                font = font
            };

            if (!initialized)
            {
                InputHandler.GetInstance().onMousePressed += (s, e) =>
                {
                    var status = InputHandler.GetStatus();
                    if (status.mouseButtonLeft && new Rectangle(position.X, position.Y, size.X, size.Y).Contains(status.mousePosition) && onClickEvent != "")
                        UIEvents.GetEvent(onClickEvent)?.Invoke(parentScreen);
                };
            }
            initialized = true;
        }

        public override void Render()
        {
            Renderer.RenderQuad(position, size, "blank", tint: backgroundTint);
            label.Render();
        }

        public override void Update() { }
    }
}
