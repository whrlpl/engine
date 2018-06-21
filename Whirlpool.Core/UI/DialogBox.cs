using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;
using Whirlpool.Core.IO.Events;

namespace Whirlpool.Core.UI
{
    public class DialogBox : UIComponent
    {
        public Font headingFont;
        public Label headingLabel;
        public Label label;
        public Button button;

        private Vector2 padding = new Vector2(64, 64);

        public override void Init(Screen screen)
        {
            headingFont = new Font("Content\\Fonts\\Heebo-Bold.ttf", font.color, 84, font.kerning);
            label = new Label()
            {
                m_position = position + new Vector2(padding.X, size.Y / 2 - 32),
                size = size - new Vector2(padding.X * 2, size.Y),
                text = text,
                formatColor = true,
                font = font
            };
            headingLabel = new Label()
            {
                m_position = position + new Vector2(padding.X, padding.Y),
                size = size - new Vector2(padding.X, 0),
                text = text,
                formatColor = true,
                font = headingFont
            };
            button = new Button()
            {
                m_position = position + new Vector2(padding.X, size.Y - padding.Y),
                size = new Vector2(110, 40),
                text = "✔️ Got it",
                font = new Font(font.filename, Color4.White, font.size, font.kerning),
                onClickEvent = "DialogCloseEvent",
                backgroundTint = Color4.Tomato,
                centered = true
            };

            headingLabel.Init(screen);
            label.Init(screen);
            button.Init(screen);
            button.parentScreen = screen;

            UIEvents.AddEvent("DialogCloseEvent", (_screen) =>
            {
                _screen.RemoveComponent(this);
                return 0;
            });

            initialized = true;
        }

        public override void Render()
        {
            Renderer.RenderQuad(position, size, "blank", tint: tint);
            label.Render();
            headingLabel.Render();
            button.Render();
        }

        public override void Update()
        { }
    }
}
