using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;

namespace Whirlpool.Core.UI
{
    public class Notification : UIComponent
    {
        public Label label;
        private long initTime;
        private Label notificationLabel;

        public override void Init(Screen screen)
        {
            if (initialized) return;
            label = new Label()
            {
                position = position + new Vector2(0, 25),
                text = text,
                font = font
            };
            notificationLabel = new Label()
            {
                position = position + new Vector2(0, 25),
                text = "Notification",
                font = font
            };
            initTime = Time.GetMilliseconds();
            initialized = true;
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
            // TODO: change position based on time. maybe play whoosh sound on exit. also check for pressed and do something

            var timeOffset = Time.GetMilliseconds() - initTime;
            if (timeOffset <= 2000)
                this.position = new Vector2((float)Math.Sin(timeOffset) / 10, 0);
            if (timeOffset >= 8000)
                this.position = new Vector2(8000 - (timeOffset / 10), 0);
        }
    }
}
