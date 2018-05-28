using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;
using Whirlpool.Game.Logic;

namespace Whirlpool.Game
{
    // TODO: Move this all to bytecode
    public class OnClickEvents
    {
        public static void Register()
        {
            UIEvents.AddEvent("LogIn", (screen) =>
            {
                if (UserAPI.LogIn(screen.GetUIComponent("UsernameBox").text, screen.GetUIComponent("PasswordBox").text))
                {
                    screen.LoadFromFile("Content\\screens\\mainmenu.xml");
                    var user = UserAPI.GetInstance().loggedInUser;
                    screen.GetUIComponent("UserText").text = user.username;
                    screen.GetUIComponent("LevelText").text = "Level " + user.level;
                    screen.GetUIComponent("LevelBar").size = new OpenTK.Vector2(1.7f * 25, 1);
                    screen.GetUIComponent("Prime").visible = user.prime;

                    GlobalSettings.Default.lastUsername = user.username;
                    GlobalSettings.Default.Save();

                    return 1;
                }
                else
                {
                    Logging.Write("Failed to log in", LogStatus.Error);
                }
                return 0;
            });

            UIEvents.AddEvent("OpenRegister", (screen) =>
            {
                System.Diagnostics.Process.Start("https://gu3.me/oslo/signup");
                return 0;
            });

            UIEvents.AddEvent("StartGame", (screen) =>
            {
                screen.LoadFromFile("Content\\screens\\world.xml");
                return 0;
            });
        }
    }
}
