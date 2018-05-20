using Whirlpool.Core;
using Whirlpool.Core.IO;
using Whirlpool.Game.Logic;

namespace Whirlpool.Game
{
    class MainGame : BaseGame
    {
        public static World world = new World();

        #region "Game properties"
        public new string gameName = "OSLO";
        public new string gameVersion = "0.2.1";
        public new string windowTitle = "%{gamename} build %{build} | game version v%{gamever} | %{fps} fps";
        #endregion

        public override void Update() { }

        public override void Init()
        {
            ParseWindowTitle();
            OnClickEvents.Register();
            ScreenCode.Register();
            base.Init();
        }

        public override void Render() { }

        public override void OneSecondPassed() => ParseWindowTitle();

        protected void ParseWindowTitle()
        {
            Title = windowTitle
                .Replace("%{gamename}", gameName)
                .Replace("%{gamever}", gameVersion)
                .Replace("%{times}", Time.GetSeconds().ToString())
                .Replace("%{timems}", Time.GetMilliseconds().ToString())
                .Replace("%{build}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString())
                .Replace("%{fps}", framesLastSecond.ToString());
        }
    }
}
