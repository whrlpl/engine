using OpenTK;
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
        public Color color;
        public int baseCharWidth;
        public int baseCharHeight;
        public Dictionary<char, FontCharacter> characters = new Dictionary<char, FontCharacter>();
        public Font(string file, Color color_, int size_ = 16, int kerning_ = 0)
        {
            filename = file;
            color = color_;
            size = size_;
            kerning = kerning_;
            var ttf = new TrueTypeSharp.TrueTypeFont(file);
            for (char c = (char)0; c <= (char)127; ++c)
            {
                FontCharacter fontChar;
                var scale = ttf.GetScaleForPixelHeight(size);
                byte[] data = ttf.GetCodepointBitmap(c, scale, scale, out fontChar.width, out fontChar.height, out fontChar.xOffset, out fontChar.yOffset);
                if (fontChar.width <= 0 || fontChar.height <= 0)
                    continue;
                List<Color> data_colorized = new List<Color>();
                foreach (byte b in data)
                {
                    data_colorized.Add(new Color(Color.Black, b));
                }
                Texture tex = Texture.FromData(data_colorized.ToArray(), fontChar.width, fontChar.height);
                fontChar.texture = tex;
                characters.Add(c, fontChar);
            }
            baseCharHeight = characters['L'].height; // TODO: proper calculations
            baseCharWidth = characters['L'].width;
            this.ttf = ttf;
        }

        public int GetStringWidth(string text)
        {
            int stringWidth = 0;
            foreach (char c in text)
            {
                if (c == ' ')
                {
                    stringWidth += baseCharWidth;
                }
                else if (characters.ContainsKey(c))
                {
                    FontCharacter fontChar = characters[c];
                    stringWidth += fontChar.width + kerning;
                }
                else
                {
                    stringWidth += baseCharWidth;
                }
            }
            return stringWidth;
        }
    }
}
