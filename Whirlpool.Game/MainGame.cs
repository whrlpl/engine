using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core;
using Whirlpool.Core.Render;
using Whirlpool.Game.Screens;
using Whirlpool.Game.Logic;
using UI = Whirlpool.Core.UI;
using Network = Whirlpool.Core.Network;

namespace Whirlpool.Game
{
    class MainGame : BaseGame
    {
        private Screen currentScreen;
        public static System.Drawing.Size windowSize;

        public World world;

        #region "Game properties"
        public new string gameName = "OSLO";
        public new string gameVersion = "0.1.2";
        #endregion

        public override void Update()
        {
            currentScreen.Update();
            world.Update();

            if (windowSize != Size)
            {
                windowSize = Size;
            }
        }

        public override void Render()
        {
            currentScreen.Render();
        }

        public override void Init()
        {
            windowTitle = "build %{build} | game version v%{gamever} | ogl %{glver} | %{fps} fps";
            windowSize = Size;
            currentScreen = new LaunchScreen();
            currentScreen.Init();
            world = new World();
            world.Init();
            
            currentScreen.AddComponent(new UI.Label()
            {
                text = "beta build",
                font = new UI.Font("Content\\Fonts\\Montserrat-Regular.ttf", Color4.White, 32),
                position = new Vector2(10, 50)
            });
            
            base.Init();
        }
    }
}
