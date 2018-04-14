using OpenTKTest.Core.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net;

namespace OpenTKTest.Core.IO
{
    public class Analytics : Singleton<Analytics>
    {
        public static List<KeyValuePair<string, string>> deviceInfoValues = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("Processor", "Win32_Processor"),
            new KeyValuePair<string, string>("Graphics", "Win32_VideoController"),
            new KeyValuePair<string, string>("Storage", "Win32_LogicalDisk")
        };

        public new static void CreateInstance()
        {
            instance = Activator.CreateInstance<Analytics>();
            instance.Init();
        }

        protected void Init()
        {
            foreach (KeyValuePair<string, string> datakvp in GetTelemetrics())
            {
                Console.WriteLine(datakvp.Key + ": " + datakvp.Value);
            }
        }

        protected string GetDeviceInfo(string device)
        {
            var temp = "";
            var c = new ManagementClass(device);
            var moc = c.GetInstances();
            var props = c.Properties;
            foreach (ManagementObject o in moc)
            {
                foreach (PropertyData p in props)
                {
                    temp += p.Name + ": " + o.Properties[p.Name].Value?.ToString() + "\n";
                }
            }
            return temp;
        }

        public static List<KeyValuePair<string, string>> GetTelemetrics()
        {
            var tmp = new List<KeyValuePair<string, string>>();
            return tmp;
        }

        public static string GetSystemProperties()
        {
            var instance = GetInstance();
            var temp = "";
            // Get generic system information
            foreach (PropertyInfo p in typeof(Environment).GetProperties())
            {
                temp += p.Name + ": " + p.GetValue(p.GetMethod) + "\n";
            }
            // Get other system information
            foreach (KeyValuePair<string, string> kvp in deviceInfoValues)
            {
                temp += kvp.Key + "\n";
                temp += instance.GetDeviceInfo(kvp.Value) + "\n";
            }
            return temp;
        }
    }
}
