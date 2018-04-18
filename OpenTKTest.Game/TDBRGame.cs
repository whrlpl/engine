using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKTest.Core;
using OpenTKTest.Core.Render;
using OpenTKTest.Core.Pattern;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core.IO;
using OpenTKTest.Game.Screens;

namespace OpenTKTest.Game
{
    class TDBRGame : BaseGame
    {
        //private OpenTKTest.Core.IO.Object testModel;
        private Screen currentScreen;

        public static System.Drawing.Size windowSize;

        public override void Update()
        {
            currentScreen.Update();
            if (windowSize != Size)
            {
                windowSize = Size;
            }
        }

        public override void Render()
        {
            BaseRenderer.RenderQuad(new Vector2(0, 0), new Vector2(Size.Width, Size.Height), "blank", Color4.Black);
            BaseRenderer.RenderGradient(new Vector2(0, 0), new Vector2(Size.Width, Size.Height));
            currentScreen.Render();
            //tertiaryLabel.Render();
        }

        public override void Initialize()
        {
            //testModel = ObjLoader.Load("Content\\lamborghini.obj");
            this.windowTitle = "idk %{gamever} | ogl %{glver} | %{fps} fps";
            windowSize = Size;
            currentScreen = new LaunchScreen();
            currentScreen.Init();
            // Hook in to add a special watermark render component
            currentScreen.AddComponent(new UI.Label()
            {
                text = "beta build",
                font = new UI.Font("Content\\Fonts\\Catamaran-Light.ttf", Color4.White, 32),
                position = new Vector2(10, 50)
            });
            //tertiaryLabel = new UI.Label()
            //{
            //    text = "Play Solo\nPlay Duo\nPlay Squad\nOptions\nExit",
            //    font = new UI.Font("Content\\Fonts\\DefaultFont.ttf", Color4.White, 24),
            //    position = new Vector2(10, 145)
            //};
        }
    }
}
