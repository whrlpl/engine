using System;

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
        /// <summary>
        /// Write a message to the console.
        /// </summary>
        /// <param name="str">The string to write.</param>
        /// <param name="status">The severity / type of the message.</param>
        public static void Write(string str, LogStatus status = LogStatus.General)
        {
            Console.WriteLine("[" + status.ToString() + "] " + str);
        }
    }
}
