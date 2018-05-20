using System.Net.Http;
using Whirlpool.Core;
using Whirlpool.Core.IO;
using Whirlpool.Core.UI;

namespace Whirlpool.Game
{
    public class ScreenCode
    {
        public static void Register()
        {
            ScreenEvents.AddEvent("OnLoad", (screen) =>
            {
                if (screen.name == "Content\\Screens\\splash.xml") screen.GetUIComponent("UsernameBox").text = GlobalSettings.Default.lastUsername;
                else if (screen.name == "Content\\Screens\\mainmenu.xml")
                {
                    // Get MOTD
                    var response = new HttpClient().PostAsync("http://gu3.me/oslo/motd.txt", null);
                    var responseString = response.Result.Content.ReadAsStringAsync();
                    Logging.Write("MOTD: " + responseString.Result);
                    var separatedResponse = responseString.Result.Split('\n');
                    ((DialogBox)screen.GetUIComponent("MOTDBox")).headingLabel.text = separatedResponse[0];
                    ((DialogBox)screen.GetUIComponent("MOTDBox")).label.text = separatedResponse[1];
                }
                else if (screen.name == "Content\\Screens\\world.xml") MainGame.world.Init();
                else return 1;
                return 0;
            });
            ScreenEvents.AddEvent("OnUpdate", (screen) =>
            {
                if (screen.name != "Content\\Screens\\world.xml") return 1;
                MainGame.world.Update();
                return 0;
            });
            ScreenEvents.AddEvent("OnRender", (screen) =>
            {
                if (screen.name != "Content\\Screens\\world.xml") return 1;
                MainGame.world.Render();
                return 0;
            });
        }
    }
}
