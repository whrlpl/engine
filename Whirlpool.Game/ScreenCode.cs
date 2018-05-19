using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core;
using Whirlpool.Core.IO;

namespace Whirlpool.Game
{
    public class ScreenCode
    {
        public static void Register()
        {
            ScreenEvents.AddEvent("OnLoad", (screen) =>
            {
                if (screen.name != "Content\\splash.xml") return 1;
                screen.GetUIComponent("UsernameBox").text = GlobalSettings.Default.lastUsername;
                return 0;
            });
        }
    }
}
