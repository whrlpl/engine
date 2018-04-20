using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.Pattern
{
    public class Singleton<T>
    {
        protected static T instance;

        public static T GetInstance()
        {
            if (instance == null)
                CreateInstance();
            return instance;
        }

        public static void CreateInstance()
        {
            instance = Activator.CreateInstance<T>();
        }
    }
}
