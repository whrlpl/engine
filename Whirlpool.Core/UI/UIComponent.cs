using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.UI
{
    public abstract class UIComponent : RenderComponent
    {
        public string text = string.Empty;
        public Font font;
        public Color4 tint = Color4.Black;
        public Vector2 position = Vector2.Zero;
        public Vector2 size = Vector2.Zero;
        public bool centered = false;
    }
}
