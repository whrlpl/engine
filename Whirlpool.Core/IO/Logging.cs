using System;
using System.Diagnostics;

namespace Whirlpool.Core.IO
{
    public enum LogStatus
    {
        Warning,
        Error,
        Critical,
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
            string formattedStr = "[" + DateTime.Now.ToString() + "] [" + status.ToString() + "] " + str;
            switch (status)
            {
                case LogStatus.Error:
                    {
                        StackTrace stackTrace = new StackTrace();
                        string frameMethodName = "";
                        for (int i = 0; i < stackTrace.FrameCount; ++i)
                        {
                            frameMethodName += stackTrace.GetFrame(i).GetMethod().Name;
                            if (i != stackTrace.FrameCount - 1)
                                frameMethodName += " --> ";
                        }
                        formattedStr += "\n\tFrom " + frameMethodName;
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    break;
                case LogStatus.Critical:
                    {
                        StackTrace stackTrace = new StackTrace();
                        string frameMethodName = "";
                        for (int i = 0; i < stackTrace.FrameCount; ++i)
                        {
                            frameMethodName += stackTrace.GetFrame(i).GetMethod().Name;
                            if (i != stackTrace.FrameCount - 1)
                                frameMethodName += " --> ";
                        }
                        formattedStr += "\n\tFrom " + frameMethodName;
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        throw new Exception("Critical error - game cannot continue: " + formattedStr);
                    }
                    break;
                case LogStatus.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            OnWrite?.Invoke(null, new LogEventArgs()
            {
                loggedStr = formattedStr
            });
            Console.WriteLine(formattedStr);

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
