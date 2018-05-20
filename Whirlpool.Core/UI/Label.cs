using System;
using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.UI
{
    public class Label : UIComponent
    {
        public int lineSpacing = 0;
        public bool formatColor = false;
        private RenderToTexture rtt;


        public override Vector2 CalculateCenterPos(Vector2 point)
        {
            if (centered)
            {
                return new Vector2(point.X - (font.GetStringSize(text).X / 2), point.Y - (font.baseCharHeight / 2));
            }
            return point;
        }

        public override void Init(Screen screen)
        {
            var stringSize = font.GetStringSize(text);
            var bounds = new Rectangle(position.X, position.Y, stringSize.X, font.baseCharHeight);
            if (initialized)
            {
                InputHandler.GetInstance().onMousePressed += (s, e) =>
                {
                    var status = InputHandler.GetStatus();
                    if (status.mouseButtonLeft)
                    {
                        if (!bounds.Contains(status.mousePosition)) return;
                        focused = true;
                        if (onClickEvent != "") UIEvents.GetEvent(onClickEvent)?.Invoke(parentScreen);
                    }
                };
                size = font.GetStringSize(text);
            }
            initialized = true;
        }

        public override void Render()
        {
            if (string.IsNullOrEmpty(text)) return;
            //float x = 0, y = 0;
            float x = position.X;
            float y = position.Y;
            Color4 color = font.color;
            if (font == null) throw new Exception("No font attached to label.");

            for (int i = 0; i < text.Length; ++i)
            {
                int c = GetUtf32Char(text, i);
                bool forceEmoji = false; // try this as default true ;)
                // HACK: hey just check if the next character is FE0F so we know whether to present it as an emoji

                if (i < text.Length - 1)
                    forceEmoji = (GetUtf32Char(text, i + 1) == 0x0000FE0F); // yes it is an emoji

                if (c >= 10000) forceEmoji = true;

                switch (c)
                {
                    case ' ':
                        x += font.baseCharWidth / 2 + font.kerning;
                        break;
                    case '\n':
                        x = position.X;
                        y += font.baseCharHeight + 10 + lineSpacing;
                        break;
                    default:
                        if (c == '*' && formatColor)
                        {
                            if (color == Color4.Tomato) color = Color4.White;
                            else if (color == Color4.White) color = Color4.Tomato;
                            continue;
                        }
                        Character fontChar = font.GetCharacter(c, forceEmoji);
                        BaseRenderer.RenderQuad(
                            position: (fontChar.type == CharacterType.Standard) ? new Vector2(x + fontChar.xOffset, y + font.baseCharHeight + (fontChar.yOffset)) : new Vector2(x + fontChar.xOffset, y - (font.size / 2) + (fontChar.yOffset)),
                            size: new Vector2(fontChar.width, fontChar.height),
                            texture: fontChar.texture,
                            flipMode: (fontChar.type == CharacterType.Standard) ? FlipMode.FlipY : FlipMode.None,
                            tint: color
                        );
                        if (i != text.Length - 1)
                            if (fontChar.type == CharacterType.Standard)
                                x += (int)(fontChar.width) + fontChar.xOffset - font.ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + font.kerning;
                            else if (fontChar.type == CharacterType.Emoji)
                                x += font.size + fontChar.xOffset - font.ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + font.kerning;
                        break;
                }

                //if (size.Y > 0 && y > size.Y) return;

                if (forceEmoji) ++i; // we shouldn't render the variation selector char
            }
        }

        public void RenderTexture()
        {
            if (rtt == null) return;
            float x = position.X;
            float y = position.Y;
            if (centered)
            {
                var fontTextMeasured = font.GetStringSize(text);
                x = position.X - (fontTextMeasured.X / 2) - font.baseCharWidth;
                y = position.Y - (fontTextMeasured.Y / 2);
            }
            BaseRenderer.RenderQuad(new Vector2(x, y), new Vector2(rtt.texture.width, rtt.texture.height), rtt.texture, Color4.White);
        }

        public int GetUtf32Char(string text, int pos)
        {
            int c = text[pos];
            // Determine whether the character at the position is a surrogate
            if (c > 0xD800 && c < 0xDFFF && pos < text.Length - 1)
            {
                // Calculate utf-32 codepoint
                int highSurrogate = text[pos] - 0xD800;
                highSurrogate = highSurrogate * 0x400;
                int lowSurrogate = text[pos + 1] - 0xDC00;
                return highSurrogate + lowSurrogate + 0x10000;
            }
            else
            {
                return c;
            }
        }

        public override void Update()
        {
            // Nothing to do here.
        }
    }
}
