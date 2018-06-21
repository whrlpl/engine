using System;
using System.Collections.Generic;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Render;

namespace Whirlpool.Core.IO.Events
{
    public class ScreenEvents : Singleton<ScreenEvents>
    {
        private Dictionary<string, Func<Screen, int>> events = new Dictionary<string, Func<Screen, int>>();

        public static Func<Screen, int> GetEvent(string name)
        {
            var instance = GetInstance();
            if (instance.events.ContainsKey(name))
                return instance.events[name];
            else
                Logging.Write("Event " + name + " not found.", LogStatus.Warning);
            return null;
        }

        public static void AddEvent(string name, Func<Screen, int> event_)
        {
            var instance = GetInstance();

            if (instance.events.ContainsKey(name))
                instance.events.Remove(name);
            instance.events.Add(name, event_);
        }
    }
}
