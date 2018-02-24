using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKTest;
using OpenTKTest.Render;
using OpenTKTest.Pattern;
using OpenTK.Graphics;

namespace GameLogic
{
    class TDBRGame : BaseGame
    {
        private UI.Label testLabel, secondaryLabel, tertiaryLabel;
        public override void Update()
        {
            
        }

        public override void Render()
        {
            //BaseRenderer.RenderQuad(new Vector2(128, 128), new Vector2(128, 128), "Content\\Blocks\\blastwave.png");
            //BaseRenderer.RenderQuad(new Vector2(628, 628), new Vector2(128, 128), "Content\\Blocks\\guns and fries.png");
            BaseRenderer.RenderQuad(new Vector2(0, 0), new Vector2(Size.Width, Size.Height), "blank", OpenTK.Graphics.Color4.Blue);
            testLabel.Render();
            secondaryLabel.Render();
            tertiaryLabel.Render();
        }

        public override void Initialize()
        {
            this.windowTitle = "idk %{gamever} | ogl %{glver} | %{fps} fps";
            testLabel = new UI.Label()
            {
                text = "he was already dead",
                font = new UI.Font("Content\\Fonts\\Asimov.ttf", Color4.White, 72, 1),
                position = new Vector2(10, 50)
            };

            secondaryLabel = new UI.Label()
            {
                text = "do i look like i know what a jpeg is",
                font = new UI.Font("Content\\Fonts\\DefaultFont.ttf", Color4.White, 48, 1),
                position = new Vector2(10, 105)
            };

            tertiaryLabel = new UI.Label()
            {
                text = "bing bing bong\ni think maybe i actually got font rendering right this time!!",
                font = new UI.Font("Content\\Fonts\\Raleway-Regular.ttf", Color4.White, 24),
                position = new Vector2(10, 145)
            };
        }
    }
}
