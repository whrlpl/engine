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
                if (args.Length > 0 && args[0] == "-nc")
                    UnsafeNativeMethods.HideConsole();
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
