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
using OpenTK.Graphics.OpenGL4;
using OpenTKTest.IO;

namespace GameLogic
{
    class TDBRGame : BaseGame
    {
        private UI.Label testLabel, secondaryLabel, tertiaryLabel;
        private OpenTKTest.IO.Object testModel;
        public override void Update()
        {
            
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(new Vector2(0, 0), new Vector2(Size.Width, Size.Height), "blank", Color4.Black);
            BaseRenderer.RenderModel(testModel, new Vector3(0, 0.0f, 2.0f), new Vector3(0.2f, 0.2f, 0.2f));
            testLabel.Render();
            secondaryLabel.Render();
            tertiaryLabel.Render();
        }

        public override void Initialize()
        {
            List<Color4> worldCloudData = new List<Color4>();
            Random r = new Random();
            int resolution = 4;
            for (int i = 0; i < Math.Pow(resolution, 2); ++i)
            {
                // Round to nearest
                byte cloudColor = (byte)(r.Next(0, 2) * 255);
                worldCloudData.Add(new Color4(cloudColor, cloudColor, cloudColor, 255));
            }

            testModel = ObjLoader.Load("Content\\monkey.obj");
            testModel.GenerateBuffers();

            this.windowTitle = "idk %{gamever} | ogl %{glver} | %{fps} fps";

            testLabel = new UI.Label()
            {
                text = "TEST GAME",
                font = new UI.Font("Content\\Fonts\\Asimov.ttf", Color4.White, 72, 25),
                position = new Vector2(10, 50)
            };

            secondaryLabel = new UI.Label()
            {
                text = "cool subtext",
                font = new UI.Font("Content\\Fonts\\Asimov.ttf", Color4.White, 48, 1),
                position = new Vector2(10, 105)
            };

            tertiaryLabel = new UI.Label()
            {
                text = "Play Solo\nPlay Duo\nPlay Squad\nOptions\nExit",
                font = new UI.Font("Content\\Fonts\\DefaultFont.ttf", Color4.White, 24),
                position = new Vector2(10, 145)
            };
        }
    }
}
