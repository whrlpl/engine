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
            // Really not optimized
            // Should render to a texture, then render to screen and generate only when the ext is changed instead of on every single draw!
            if (string.IsNullOrEmpty(text)) return;

            float x = position.X;
            float y = position.Y;
            for (int i = 0; i < text.Length; ++i)
            {
                var c = text[i];
                if (c == ' ')
                {
                    x += font.baseCharWidth;
                }
                if (c == '\n')
                {
                    x = position.X;
                    y += (int)((font.baseCharHeight) + 10);
                }
                else if (font.characters.ContainsKey(c))
                {
                    FontCharacter fontChar = font.characters[c];
                    //var offset = 0.0f;
                    //if (i > 0)
                    //    offset = font.ttf.GetCodepointKernAdvance(text[i - 1], text[i]);
                    BaseRenderer.RenderQuad(new Vector2(x + fontChar.xOffset, y + font.baseCharHeight + (fontChar.yOffset)), new Vector2(fontChar.width, fontChar.height), fontChar.texture, flipMode: FlipMode.FlipY);
                    if (i != text.Length - 1)
                        x += (int)(fontChar.width) + fontChar.xOffset - (font.ttf.GetCodepointKernAdvance(c, text[i + 1]) * font.kerning) + (int)font.kerning; // (int)(fontChar.width) + (int)(font.kerning);
                }
            }
        }
    }
}
