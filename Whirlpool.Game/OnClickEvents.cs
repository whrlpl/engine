using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
                Console.WriteLine("Log in event called");
                if (UserAPI.LogIn(screen.GetUIComponent("UsernameBox").text, screen.GetUIComponent("PasswordBox").text))
                {
                    screen.LoadFromFile("Content\\Screens\\mainmenu.xml");
                    screen.GetUIComponent("UserText").text = UserAPI.GetInstance().loggedInUser.username;
                    screen.GetUIComponent("LevelText").text = "Level 095";
                    screen.GetUIComponent("LevelBar").size = new OpenTK.Vector2(1.7f*25, 1);
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
                Console.WriteLine("Register event called");
                System.Diagnostics.Process.Start("https://gu3.me/oslo/signup");
                return 0;
            });
        }
    }
}
