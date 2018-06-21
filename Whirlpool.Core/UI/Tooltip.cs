using System;
using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.UI
{
    public class Tooltip : UIComponent
    {
        public Label label;

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
        }

        public override void Render()
        {
            label.text = text;
            var tempPosition = position + new Vector2(0, 16);
            if (position.X > BaseGame.Size.Width - 250)
            {
                tempPosition.X -= size.X;
            }
            label.m_position = tempPosition + new Vector2(8, font.baseCharHeight * 2);
            size = new Vector2(font.GetStringSize(text).X + 24, font.baseCharHeight * 3 + 16);
            Renderer.RenderQuad(tempPosition, size, "blank", tint: new Color4(0, 0, 0, 150));
            label.Render();
        }

        public override void Update() { }
    }
}
