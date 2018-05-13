using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;

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
                Console.WriteLine("Log in as " + screen.GetUIComponent("UsernameBox").text + " with password " + screen.GetUIComponent("PasswordBox").text);
                screen.LoadFromFile("Content\\Screens\\mainmenu.xml");
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
