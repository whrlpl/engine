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
    class Button : RenderComponent
    {
        public string text = string.Empty;
        public Font font;
        public Color4 tint = Color4.Black;
        public Vector2 position = Vector2.Zero;
        public Vector2 size = Vector2.Zero;
        public bool centered = false;
        public Label label;

        public override void Init()
        {
            label = new Label()
            {
                centered = this.centered,
                position = this.position,
                text = this.text,
                font = this.font,
                tint = Color4.Black
            };
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, "blank", tint: new Color4(150, 150, 150, 150));
            label.Render();
        }

        public override void Update()
        {
            // TODO: Check for mouse input and call onClick
        }
    }
}
