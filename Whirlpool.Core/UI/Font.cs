﻿using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;
using Whirlpool.Core.IO.Assets;

namespace Whirlpool.Core.UI
{

    /* TODO: huge rethink of font and label system
     * maybe include some sort of fontbank
     * use a bytecode API mixed with the UI file format
     * this should make everything more streamlined and
     * much better optimized! */

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
            if (!characters.ContainsKey(c))
                characters[c] = RequestCharGeneration(c, forceEmoji);
            return characters[c];
        }

        private Character RequestCharGeneration(int c, bool forceEmoji = false) // forceEmoji for those annoying af pre-1f600 chars with fe0f
        {
            if (!characters.ContainsKey(c))
            {
                if (c > 0x0001F600 || forceEmoji) // utf-32!!!!!
                {
                    var emojiLoc = "Content\\emoji\\" + c.ToString("X").ToLower() + ".png";
                    Logging.Write("Loading emoji " + emojiLoc);
                    Texture tex = TextureLoader.LoadAsset(emojiLoc);
                    tex.textureWrapMode = OpenTK.Graphics.OpenGL4.TextureWrapMode.ClampToBorder;

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
                    List<Color4> dataColorized = new List<Color4>();
                    var scale = ttf.GetScaleForPixelHeight(size);
                    byte[] data = ttf.GetCodepointBitmap((char)c, scale, scale, out fontChar.width, out fontChar.height, out fontChar.xOffset, out fontChar.yOffset);

                    for (int i = 0; i < data.Length; ++i)
                    {
                        var b = data[i];
                        dataColorized.Add(new Color4(b, b, b, b));
                    }

                    Texture tex = Texture.FromData(dataColorized.ToArray(), fontChar.width, fontChar.height);
                    tex.name = filename + "_" + c;
                    tex.textureWrapMode = OpenTK.Graphics.OpenGL4.TextureWrapMode.ClampToBorder;
                    fontChar.texture = tex;
                    fontChar.type = CharacterType.Standard;

                    return fontChar;
                }
            }
            return null;
        }
        
        public Vector2 GetStringSize(string text) // TODO: really need some way of making it so that there's only 1 set of this code (see label.cs)
        {
            float x = 0;
            int numLines = 1;
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
                        x += baseCharWidth / 2 + kerning;
                        break;
                    case '\n':
                        numLines++;
                        break;
                    default:
                        Character fontChar = GetCharacter(c, forceEmoji);
                        if (i != text.Length - 1)
                            if (fontChar.type == CharacterType.Standard)
                            {
                                x += (int)(fontChar.width) + fontChar.xOffset - ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + kerning;
                            }
                            else if (fontChar.type == CharacterType.Emoji)
                            {
                                x += size + fontChar.xOffset - ttf.GetCodepointKernAdvance((char)c, text[i + 1]) + kerning;
                            }
                        break;
                }
            }
            return new Vector2(x, baseCharHeight + 10 * numLines);
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
    }
}