using System;
using System.IO;
using System.Net.Http;
using System.Xml.Linq;
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
                if (screen.name == "Content\\screens\\splash.xml") screen.GetUIComponent("UsernameBox").text = GlobalSettings.Default.lastUsername;
                else if (screen.name == "Content\\screens\\mainmenu.xml")
                {
                    // Get MOTD
                    var response = new HttpClient().PostAsync("http://gu3.me/oslo/motd.txt", null);
                    var responseString = response.Result.Content.ReadAsStringAsync();
                    Logging.Write("MOTD: " + responseString.Result);
                    var separatedResponse = responseString.Result.Split('\n');
                    ((DialogBox)screen.GetUIComponent("MOTDBox")).headingLabel.text = separatedResponse[0];
                    ((DialogBox)screen.GetUIComponent("MOTDBox")).label.text = separatedResponse[1];
                }
                else if (screen.name == "Content\\screens\\world.xml") MainGame.world.Init();
                else if (screen.name == "Content\\screens\\update.xml")
                {
                    var currentBuildDate = File.GetCreationTime(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Accept", "application/xml");
                    var response = client.GetAsync("https://ci.appveyor.com/api/projects/LetsRaceBwoi/engine");
                    var responseString = response.Result.Content.ReadAsStringAsync();
                    XElement loaded = XElement.Load(new StringReader(responseString.Result));
                    var latestBuildDate = DateTime.Parse(GetElement(loaded, "DateTime").Value);
                    if (latestBuildDate > currentBuildDate)
                        Logging.Write("Update available", LogStatus.Warning);
                }
                else return 1;
                return 0;
            });
            ScreenEvents.AddEvent("OnUpdate", (screen) =>
            {
                if (screen.name != "Content\\screens\\world.xml") return 1;
                MainGame.world.Update();
                return 0;
            });
            ScreenEvents.AddEvent("OnRender", (screen) =>
            {
                if (screen.name != "Content\\screens\\world.xml") return 1;
                MainGame.world.Render();
                return 0;
            });
        }

        public static XElement GetElement(XElement root, string target)
        {
            foreach (var e in root.Elements())
            {
                if (e.HasElements)
                {
                    var childElement = GetElement(e, target);
                    if (childElement != null) return childElement;
                }
                if (e.Name.LocalName.ToString() == target)
                    return e;
            }
            return null;
        }
    }
}
