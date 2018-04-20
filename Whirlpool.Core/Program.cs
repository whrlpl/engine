using System;

namespace OpenTKTest
{
    public class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
            using (var g = new BaseGame())
                g.Run();
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
