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
    public struct FontCharacter
    {
        public int width;
        public int height;
        public int xOffset;
        public int yOffset;
        public Texture texture;
    }

    public class Font
    {
        public TrueTypeSharp.TrueTypeFont ttf;
        public int size;
        public int kerning;
        public string filename;
        public Color4 color;
        public int baseCharWidth;
        public int baseCharHeight;
        public Dictionary<char, FontCharacter> characters = new Dictionary<char, FontCharacter>();
        public Font(string file, Color4 color_, int size_ = 16, int kerning_ = 0)
        {
            filename = file;
            color = color_;
            size = size_;
            kerning = kerning_;
            ttf = new TrueTypeSharp.TrueTypeFont(file);
            for (int c = 0; c <= 127; ++c)
            {
                RequestCharGeneration((char)c);
            }
            baseCharHeight = characters['L'].height;
            baseCharWidth = characters['L'].width;
        }

        public void RequestCharGeneration(char c)
        {
            if (!characters.ContainsKey(c))
            {
                // Load character
                FontCharacter fontChar;
                List<Color4> data_colorized = new List<Color4>();
                var scale = ttf.GetScaleForPixelHeight(size);
                byte[] data = ttf.GetCodepointBitmap((char)c, scale, scale, out fontChar.width, out fontChar.height, out fontChar.xOffset, out fontChar.yOffset);

                foreach (byte b in data)
                    data_colorized.Add(new Color4(0, 0, 0, b));

                Texture tex = Texture.FromData(data_colorized.ToArray(), fontChar.width, fontChar.height);
                tex.name = filename + "_" + c;
                fontChar.texture = tex;
                characters.Add(c, fontChar);
            }
        }

        public Vector2 GetStringSize(string text)
        {
            float x = 0, y = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                var c = text[i];
                switch (c)
                {
                    case ' ':
                        x += baseCharWidth / 2 + kerning;
                        break;
                    case '\n':
                        x = 0;
                        y += baseCharHeight + 10;
                        break;
                    default:
                        if (characters.ContainsKey(c))
                        {
                            FontCharacter fontChar = characters[c];
                            if (i != text.Length - 1)
                                x += (int)(fontChar.width) + fontChar.xOffset - ttf.GetCodepointKernAdvance(c, text[i + 1]) + (int)kerning;
                        }
                        break;
                }
            }
            return new Vector2(x, y);
        }
    }
}
