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
            var ttfInstance = new TrueTypeSharp.TrueTypeFont(file);
            for (int c = 0; c <= 127; ++c)
            {
                FontCharacter fontChar;
                List<Color4> data_colorized = new List<Color4>();
                var scale = ttfInstance.GetScaleForPixelHeight(size);
                byte[] data = ttfInstance.GetCodepointBitmap((char)c, scale, scale, out fontChar.width, out fontChar.height, out fontChar.xOffset, out fontChar.yOffset);

                if (fontChar.width <= 0 || fontChar.height <= 0)
                    continue;
                foreach (byte b in data)
                    data_colorized.Add(new Color4(0, 0, 0, b));

                Texture tex = Texture.FromData(data_colorized.ToArray(), fontChar.width, fontChar.height);
                tex.name = file + "_" + c;
                fontChar.texture = tex;
                characters.Add((char)c, fontChar);
            }
            baseCharHeight = characters['L'].height;
            baseCharWidth = characters['L'].width;
            ttf = ttfInstance;
        }
    }
}
