using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Game.UI
{
    class Notification : RenderComponent
    {
        public string text = string.Empty;
        public Font font = null;
        public Color4 tint = Color4.Black;
        public Vector2 position = Vector2.Zero;
        public Vector2 size = Vector2.Zero;
        public Label label;

        private Label notificationLabel;

        public override void Init()
        {
            label = new Label()
            {
                position = this.position + new Vector2(0, 100),
                text = this.text,
                font = this.font,
                tint = Color4.Black
            };
            notificationLabel = new Label()
            {
                position = this.position,
                text = "Notification",
                font = this.font,
                tint = Color4.Black
            };
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, "Content\\notification.png", Color4.White);
            notificationLabel.Render();
            label.Render();
        }

        public override void Update()
        {
            // TODO: change position based on time. maybe play woosh sound on exit. also check for pressed and do something
        }
    }
}
