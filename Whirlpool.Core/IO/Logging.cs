using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.IO
{
    public enum LogStatus
    {
        Warning,
        Error,
        General
    }

    public class Logging
    {
        public static void Write(string str, LogStatus status = LogStatus.General)
        {
            Console.WriteLine("[" + status.ToString() + "] " + str);
        }
    }
}
