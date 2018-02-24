﻿using System;
using OpenTKTest;

namespace GameLogic
{
    public class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            try
            {
#endif
            using (var g = new TDBRGame())
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
