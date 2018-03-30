using OpenTK;
using OpenTK.Graphics;
using OpenTKTest.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.UI
{
    class Label : RenderComponent
    {
        public string text = string.Empty;
        public int lineSpacing;
        public Font font;
        public Color4 tint = Color4.Black;
        public Vector2 position = Vector2.Zero;
        public bool centered = false;

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
                var c = text[i];
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
                        if (!font.characters.ContainsKey(c))
                            font.RequestCharGeneration(c);
                            FontCharacter fontChar = font.characters[c];
                            BaseRenderer.RenderQuad(new Vector2(x + fontChar.xOffset, y + font.baseCharHeight + (fontChar.yOffset)), new Vector2(fontChar.width, fontChar.height), fontChar.texture, flipMode: FlipMode.FlipY, tint: tint);
                            if (i != text.Length - 1)
                                x += (int)(fontChar.width) + fontChar.xOffset - font.ttf.GetCodepointKernAdvance(c, text[i + 1]) + font.kerning;
                        break;
                }
            }
        }

        public override void Update()
        {
            // Nothing to do here.
        }
    }
}
