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
            switch (status)
            {
                case LogStatus.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogStatus.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.WriteLine("[" + status.ToString() + "] " + str);
            
            string emojiPrefix = "";

            if (BaseGame.consoleScreen != null)
            {
                switch (status)
                {
                    case LogStatus.Error:
                        emojiPrefix = "❌";
                        break;
                    case LogStatus.Warning:
                        emojiPrefix = "⚠";
                        break;
                }
                var logTextComponent = BaseGame.consoleScreen.GetUIComponent("LogText");
                if (logTextComponent != null)
                    logTextComponent.text += "*[" + status.ToString() + "]* " + emojiPrefix + " " + str + "\n";
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
