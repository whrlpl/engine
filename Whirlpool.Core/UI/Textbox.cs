using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.UI
{
    public class Textbox : UIComponent
    {
        private Label label;
        private Label placeholderLabel;
        public string placeholder;
        public bool isPassword;

        public void SetPlaceholder(string newPlaceholder)
        {
            placeholder = newPlaceholder;
        }

        public void SetIsPassword(bool newIsPassword)
        {
            isPassword = newIsPassword;
        }

        public override void Init()
        {
            label = new Label()
            {
                centered = centered,
                position = position,
                text = text,
                font = font
            };
            placeholderLabel = new Label()
            {
                centered = centered,
                position = position,
                text = text,
                font = font
            };
            size = new Vector2(Math.Max(size.X, font.GetStringSize(label.text).X) + 16, font.baseCharHeight * 3 + 16);
            position = new Vector2(position.X - 8, position.Y - font.baseCharHeight - 8);
            var bounds = new Rectangle(position.X, position.Y, size.X, size.Y);
            InputHandler.GetInstance().onMousePressed += (s, e) =>
            {
                var status = InputHandler.GetStatus();
                if (status.mouseButtonLeft)
                {
                    focused = (bounds.Contains(status.mousePosition));
                }
            };

            InputHandler.GetInstance().onKeyPressed += (s, e) =>
            {
                var status = InputHandler.GetStatus();
                if (!focused) return;
                if (e.pressed)
                {
                    switch (e.key)
                    {
                        case Key.Number0: case Key.Number1: case Key.Number2: case Key.Number3: case Key.Number4:
                        case Key.Number5: case Key.Number6: case Key.Number7: case Key.Number8: case Key.Number9:
                            text += e.key.ToString()[6];
                            break;
                        case Key.Space:
                            text += " ";
                            break;
                        case Key.BackSpace:
                            if (text.Length > 0)
                                text = text.Remove(text.Length - 1);
                            break;
                        case Key.ShiftLeft: case Key.ShiftRight: break;
                        default:
                            if (e.key >= Key.A && e.key <= Key.Z)
                            text += (status.shift) ? e.key.ToString()[0] : e.key.ToString().ToLower()[0];
                            break;
                    }
                }
            };
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, "blank", tint: new Color4(150, 150, 150, 150));
            if (text == "") placeholderLabel.Render();
            label.Render();
        }

        public override void Update()
        {
            string tmp = (isPassword) ? new string('•', text.Length) : text;
            label.text = (focused && Time.GetMilliseconds() % 1000 <= 500) ? tmp + "|" : tmp;
        }
    }
}
