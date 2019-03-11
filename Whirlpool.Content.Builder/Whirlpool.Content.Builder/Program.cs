using System;
using System.IO;

/* 
 * TODOs:
 * - Caching?
 * - Metadata for some files
 */


namespace Whirlpool.Content.Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            bool extract = false;
            bool verbose = false;
            bool forceRebuild = false;
            DateTime startTime = DateTime.Now;
            string folder = Directory.GetCurrentDirectory() + "\\Content";
            string extractLoc = Directory.GetCurrentDirectory() + "\\TestExtract";
            if (args.Length >= 1)
                folder = args[0];
            else
                Console.WriteLine("Folder chosen automatically");
            if (args.Length >= 2)
                foreach (string arg in args)
                {
                    switch (arg)
                    {
                        case "-extract":
                            extract = true;
                            break;
                        case "-verbose":
                            verbose = true;
                            break;
                        case "-force-rebuild":
                            forceRebuild = true;
                            break;
                    }
                }
            
            using (var contentBuilder = new ContentBuilder(folder, verbose))
            {
                Console.WriteLine("Building content in '" + folder + "'");
                contentBuilder.Compile(!System.IO.File.Exists(folder + ".wpak"), forceRebuild);
                Console.WriteLine("Finished content build");
                if (extract)
                {
                    Console.WriteLine("Extracting to '" + extractLoc + "'");
                    Directory.CreateDirectory(extractLoc);
                    contentBuilder.package.Extract(extractLoc);
                    Console.WriteLine("Finished extraction");
                }
                DateTime endTime = DateTime.Now;
                Console.WriteLine("Completed in " + (endTime - startTime).TotalSeconds + "s.");
            }
            Console.ReadLine();
        }
    }
}
