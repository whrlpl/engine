using OpenTK;
using OpenTKTest.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.UI
{
    class Label
    {
        public string text = string.Empty;
        public Font font;
        public Vector2 position = Vector2.Zero;

        public void Render()
        {
            // !!! Really not optimized !!!
            // Should render to a texture, then render to screen and generate only when the ext is changed instead of on every single draw!
            if (string.IsNullOrEmpty(text)) return;
            if (font == null) throw new Exception("No font attached to label.");

            float x = position.X;
            float y = position.Y;
            for (int i = 0; i < text.Length; ++i)
            {
                var c = text[i];
                switch (c)
                {
                    case ' ':
                        x += font.baseCharWidth / 2 + font.kerning;
                        break;
                    case '\n':
                        x = position.X;
                        y += font.baseCharHeight + 10;
                        break;
                    default:
                        if (font.characters.ContainsKey(c))
                        {
                            FontCharacter fontChar = font.characters[c];
                            BaseRenderer.RenderQuad(new Vector2(x + fontChar.xOffset, y + font.baseCharHeight + (fontChar.yOffset)), new Vector2(fontChar.width, fontChar.height), fontChar.texture, flipMode: FlipMode.FlipY);
                            if (i != text.Length - 1)
                                x += (int)(fontChar.width) + fontChar.xOffset - font.ttf.GetCodepointKernAdvance(c, text[i + 1]) + (int)font.kerning;
                        }
                        break;
                }
            }
        }
    }
}
