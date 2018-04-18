using OpenTK;
using OpenTK.Graphics;
using OpenTKTest.Core.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKTest.Game.UI
{
    public enum CharacterType
    {
        Standard,
        Emoji,
        Other
    }

    public class Character
    {
        public int width;
        public int height;
        public int xOffset;
        public int yOffset;
        public CharacterType type;
        public Texture texture;
    }

    public class Font
    {
        public TrueTypeSharp.TrueTypeFont ttf;
        public int size;
        public int kerning;
        public string filename;
        public int baseCharWidth;
        public int baseCharHeight;
        public Color4 color;
        public Dictionary<int, Character> characters = new Dictionary<int, Character>();

        public Font(string file, Color4 color_, int size_ = 16, int kerning_ = 0)
        {
            filename = file;
            color = color_;
            size = size_;
            kerning = kerning_;
            ttf = new TrueTypeSharp.TrueTypeFont(file);
            var baseCharSize = GetBaseCharSize();
            baseCharHeight = baseCharSize.Width;
            baseCharWidth = baseCharSize.Height;
        }

        public Size GetBaseCharSize()
        {
            var tmp = RequestCharGeneration('L');
            return new Size(tmp.width, tmp.height);
        }

        public Character GetCharacter(int c, bool forceEmoji = false) 
        {
            // TODO: forceEmoji here will only work if we're getting the character for the first time - should not be a problem, but fix anyway!!!
            if (!characters.ContainsKey(c)) characters[c] = RequestCharGeneration(c, forceEmoji);
            return characters[c];
        }

        private Character RequestCharGeneration(int c, bool forceEmoji = false) // forceEmoji for those annoying af pre-1f600 chars with fe0f
        {
            if (!characters.ContainsKey(c))
            {
                if (c > 0x0001F600 || forceEmoji) // utf-32!!!!!
                {
                    var emojiLoc = "Content\\emoji\\" + c.ToString("X").ToLower() + ".png";
                    Console.WriteLine("Loading emoji " + emojiLoc);
                    Texture tex = Texture.FromFile(emojiLoc);
                    tex.name = filename;

                    Character fontChar = new Character()
                    {
                        texture = tex,
                        width = size,
                        height = size,
                        type = CharacterType.Emoji
                    };

                    return fontChar;
                }
                else
                {
                    Character fontChar = new Character();
                    List<Color4> data_colorized = new List<Color4>();
                    var scale = ttf.GetScaleForPixelHeight(size);
                    byte[] data = ttf.GetCodepointBitmap((char)c, scale, scale, out fontChar.width, out fontChar.height, out fontChar.xOffset, out fontChar.yOffset);

                    foreach (byte b in data)
                        data_colorized.Add(new Color4(0, 0, 0, b));

                    Texture tex = Texture.FromData(data_colorized.ToArray(), fontChar.width, fontChar.height);
                    tex.name = filename + "_" + c;
                    fontChar.texture = tex;
                    fontChar.type = CharacterType.Standard;

                    return fontChar;
                }
            }
            return null;
        }
        
        public Vector2 GetStringSize(string text) // TODO: really need some way of making it so that there's only 1 set of this code (see label.cs)
                                                    // TODO (cont) - can be removed with switch to render textures
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
                            Character fontChar = characters[c];
                            if (i != text.Length - 1)
                                if (fontChar.type == CharacterType.Standard)
                                    x += (int)(fontChar.width) + fontChar.xOffset - ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + kerning;
                                else if (fontChar.type == CharacterType.Emoji)
                                    x += size + fontChar.xOffset - ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + kerning;
                        }
                        break;
                }
            }
            return new Vector2(x, y);
        }
    }
}
