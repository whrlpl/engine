using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using Whirlpool.Core;
using Whirlpool.Core.Render;
using Whirlpool.Game.Logic;
using Whirlpool.Core.IO;
using UI = Whirlpool.Core.UI;
using Network = Whirlpool.Core.Network;

namespace Whirlpool.Game
{
    class MainGame : BaseGame
    {
        public static System.Drawing.Size windowSize;

        public World world;

        #region "Game properties"
        public new string gameName = "OSLO";
        public new string gameVersion = "0.2.0";
        #endregion

        public override void Update()
        {
            world.Update();

            if (windowSize != Size)
            {
                windowSize = Size;
            }
        }

        public override void Init()
        {
            windowTitle = "OSLO build %{build} | game version v%{gamever} | ogl %{glver} | %{fps} fps";
            windowSize = Size;
            world = new World();
            world.Init();
            OnClickEvents.Register();
            ScreenCode.Register();
            base.Init();
        }

        public override void Render()
        {
            world.Render();
        }
    }
}
