using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Whirlpool.Core.Pattern;

namespace Whirlpool.Game.Logic
{
    public class User
    {
        public int level;
        public int experience;
        public string username;
        public string title;
        public bool prime;
    }

    public class UserAPI : Singleton<UserAPI>
    {
        HttpClient client = new HttpClient();
        private User currentUser;
        public User loggedInUser { get { return currentUser; } }

        public static bool LogIn(string username, string password)
        {
            var instance = GetInstance();
            var values = new Dictionary<string, string>
                {
                   { "username", username },
                   { "password", password },
                   { "type", "login" }
                };
            var content = new FormUrlEncodedContent(values);
            var response = instance.client.PostAsync("http://oslo.gu3.me/api/user.php", content);
            try
            {
                var responseString = response.Result.Content.ReadAsStringAsync();
                if (responseString.Result == "success")
                {
                    instance.currentUser = new User()
                    {
                        username = username,
                        prime = true,
                        level = 1,
                        experience = 0,
                        title = "Deep Pockets"
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
#if DEBUG
                instance.currentUser = new User()
                {
                    username = username,
                    prime = true,
                    level = 1,
                    experience = 0,
                    title = "Deep Pockets"
                };
                return true;
#else
                return false;
#endif
            }
        }
    }
}
