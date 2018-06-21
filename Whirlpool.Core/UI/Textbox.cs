using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;
using Whirlpool.Core.Render;
using Whirlpool.Core.IO.Events;
using Whirlpool.Core.IO.Assets;

namespace Whirlpool.Core.UI
{
    public class Textbox : UIComponent
    {
        private Label label;
        private Label placeholderLabel;
        private Texture bgTex;
        public string placeholder;
        public bool isPassword;
        
        public override Vector2 CalculateCenterPos(Vector2 point)
        {
            if (centered)
            {
                return new Vector2(point.X, point.Y - font.baseCharHeight) - (new Vector2(Math.Max(size.X, font.GetStringSize(label.text).X), font.baseCharHeight * 3) / 2);
            }
            return point;
        }

        public void SetPlaceholder(string newPlaceholder)
        {
            placeholder = newPlaceholder;
        }

        public void SetIsPassword(bool newIsPassword)
        {
            isPassword = newIsPassword;
        }

        public override void Init(Screen screen)
        {
            parentScreen = screen;
            var labelPos = (centered) ? position + new Vector2(8, font.baseCharHeight * 2) : position;
            size = new Vector2(size.X, font.baseCharHeight * 3 + 16);
            var bounds = new Rectangle(position.X, position.Y, size.X, size.Y);
            label = new Label()
            {
                m_position = labelPos,
                text = text,
                font = font
            };
            placeholderLabel = new Label()
            {
                m_position = labelPos,
                text = placeholder,
                font = font
            };
            bgTex = TextureLoader.LoadAsset("Content\\shadow.png");

            if (initialized)
            {
                InputHandler.GetInstance().onMousePressed += (s, e) =>
                {
                    var status = InputHandler.GetStatus();
                    if (status.mouseButtonLeft)
                    {
                        focused = (bounds.Contains(status.mousePosition));
                        if (onClickEvent != "") UIEvents.GetEvent(onClickEvent)?.Invoke(parentScreen);
                    }
                };
            }
            else
            {
                InputHandler.GetInstance().onKeyPressed += (s, e) =>
                {
                    var status = InputHandler.GetStatus();
                    if (!focused) return;
                    if (font.GetStringSize(text + "|").X >= size.X - 32 && e.key != Key.BackSpace) return;
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
            initialized = true;
        }

        public override void Render()
        {
            Renderer.RenderQuad(position, size, "blank", tint);
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
