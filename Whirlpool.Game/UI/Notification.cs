using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;

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

        private long initTime;

        private Label notificationLabel;

        public override void Init()
        {
            label = new Label()
            {
                position = this.position + new Vector2(0, 25),
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
            initTime = Time.GetMilliseconds();
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(position, size, "Content\\notification.png", Color4.White);
            notificationLabel.Render();
            label.Render();
        }

        public override void Update()
        {
            /*      |   ----------------------
             *  X   |  /                      \
             * Pos  | /                         -\
             *      |/                             -\
             *      |--------------------------------------------
             *                          Time
             * (Eases out)
             */
            // TODO: change position based on time. maybe play woosh sound on exit. also check for pressed and do something

            var timeOffset = Time.GetMilliseconds() - initTime;
            if (timeOffset <= 2000)
                this.position = new Vector2((float)Math.Sin(timeOffset) / 10, 0);
            if (timeOffset >= 8000)
                this.position = new Vector2(8000 - (timeOffset / 10), 0);
        }
    }
}
