using OpenTK;
using OpenTK.Graphics;
using OpenTKTest.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Game.UI
{
    class Label : RenderComponent
    {
        public string text = string.Empty;
        public int lineSpacing;
        public Font font;
        public Color4 tint = Color4.Black;
        public Vector2 position = Vector2.Zero;
        public bool centered = false;

        private Texture labelTexture;

        public override void Init()
        {
            // TODO: render the font to texture
        }

        public override void Render()
        {
            // !!! Really not optimized !!!
            // Should render to a texture, then render to screen and generate only when the ext is changed instead of on every single draw!
            if (string.IsNullOrEmpty(text)) return;

            float x = position.X;
            float y = position.Y;
            if (centered)
            {
                var fontTextMeasured = font.GetStringSize(text);
                x = position.X - (fontTextMeasured.X / 2) - font.baseCharWidth;
                y = position.Y - (fontTextMeasured.Y / 2);
            }
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
                        Character fontChar = font.GetCharacter(c, forceEmoji);
                        BaseRenderer.RenderQuad(
                            position: (fontChar.type == CharacterType.Standard) ? new Vector2(x + fontChar.xOffset, y + font.baseCharHeight + (fontChar.yOffset)) : new Vector2(x + fontChar.xOffset, y - (font.size  / 2) + (fontChar.yOffset)),
                            size: new Vector2(fontChar.width, fontChar.height),
                            texture: fontChar.texture,
                            flipMode: (fontChar.type == CharacterType.Standard) ? FlipMode.FlipY : FlipMode.None,
                            tint: tint
                        );
                        if (i != text.Length - 1)
                            if (fontChar.type == CharacterType.Standard)
                                x += (int)(fontChar.width) + fontChar.xOffset - font.ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + font.kerning;
                            else if (fontChar.type == CharacterType.Emoji)
                                x += font.size + fontChar.xOffset - font.ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + font.kerning;
                        break;
                }

                if (forceEmoji) ++i; // we shouldn't render the variation selector char
            }
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
            //try
            //{
            //    c = char.ConvertToUtf32(text, pos); // this is so awfully slow that its untrue
            //}
            //catch (ArgumentException ex)
            //{
            //    // probably low surrogate w/o high
            //    c = char.ConvertToUtf32(text[pos - 1], text[pos]); // also fucking terribly slow
            //}
            //return c;
        }

        public override void Update()
        {
            // Nothing to do here.
        }
    }
}
