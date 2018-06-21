using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Pattern
{
    /// <summary>
    /// Class for the singleton pattern.
    /// </summary>
    /// <typeparam name="T">The type of the singleton</typeparam>
    [ComVisible(true)]
    public class Singleton<T>
    {
        protected static T instance;

        /// <summary>
        /// Get an instance of T.
        /// </summary>
        /// <returns>An active instance of T</returns>
        public static T GetInstance()
        {
            if (instance == null)
                CreateInstance();
            return instance;
        }

        /// <summary>
        /// Create an instance of T.
        /// </summary>
        public static void CreateInstance()
        {
            instance = Activator.CreateInstance<T>();
        }
    }
}
