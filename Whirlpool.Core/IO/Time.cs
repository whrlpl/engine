using Whirlpool.Core.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.IO
{
    public class Time : Singleton<Time>
    {
        private float currentTime_ = 0;

        public static float currentTime
        {
            get
            {
                return GetInstance().currentTime_;
            }
        }

        public static void AddTime(float time)
        {
            GetInstance().currentTime_ += time;
        }

        public static int GetSeconds()
        {
            return (int)GetInstance().currentTime_;
        }
        
        public static int GetMilliseconds()
        {
            return (int)(GetInstance().currentTime_ * 1000);
        }

        public static long GetFlicks()
        {
            return (long)(GetInstance().currentTime_ * 705600000);
        }
    }
}
