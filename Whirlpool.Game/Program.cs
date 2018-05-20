using System;
using Whirlpool.Core.IO;

namespace Whirlpool.Game
{
    public class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
            using (var g = new MainGame())
            {
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-nc":
                            UnsafeNativeMethods.HideConsole();
                            break;
                        default:
                            Logging.Write("Unrecognized argument '" + arg + "'.", LogStatus.Error);
                            break;
                    }
                }
                g.Run();
            }
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
#endif
        }
    }
}
