using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Whirlpool.Core;
using Whirlpool.Core.Render;
using Whirlpool.Core.Pattern;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.IO;
using Whirlpool.Game.Screens;

namespace Whirlpool.Game
{
    class MainGame : BaseGame
    {
        //private Whirlpool.Core.IO.Object testModel;
        private Screen currentScreen;
        public static System.Drawing.Size windowSize;
        
        #region "Game properties"
        public new string gameName = "Asimov";
        public new string gameVersion = "0.1.0";
        #endregion

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
        }

        public override void Init()
        {
            windowTitle = "build %{build} | game version v%{gamever} | ogl %{glver} | %{fps} fps";
            windowSize = Size;
            currentScreen = new LaunchScreen();
            currentScreen.Init();
            
            currentScreen.AddComponent(new UI.Label()
            {
                text = "beta build",
                font = new UI.Font("Content\\Fonts\\Catamaran-Light.ttf", Color4.White, 32),
                position = new Vector2(10, 50)
            });
            
            base.Init();
        }
    }
}
