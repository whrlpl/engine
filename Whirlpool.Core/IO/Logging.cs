using System;

namespace Whirlpool.Core.IO
{
    public enum LogStatus
    {
        Warning,
        Error,
        General
    }

    public class LogEventArgs : EventArgs
    {
        public string loggedStr { get; set; }
    }

    public class Logging
    {
        public static EventHandler<LogEventArgs> OnWrite;
        /// <summary>
        /// Write a message to the console.
        /// </summary>
        /// <param name="str">The string to write.</param>
        /// <param name="status">The severity / type of the message.</param>
        public static void Write(string str, LogStatus status = LogStatus.General)
        {
            switch (status)
            {
                case LogStatus.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogStatus.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            string formattedStr = "[" + DateTime.Now.ToString() + "] [" + status.ToString() + "] " + str;
            OnWrite?.Invoke(null, new LogEventArgs()
            {
                loggedStr = formattedStr
            });
            Console.WriteLine(formattedStr);

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
