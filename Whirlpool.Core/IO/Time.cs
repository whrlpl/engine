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
        private float m_currentTime = 0;
        private double m_lastFrameTime = 0;

        public static float currentTime
        {
            get
            {
                return GetInstance().m_currentTime;
            }
        }

        public static double lastFrameTime
        {
            get
            {
                return GetInstance().m_lastFrameTime;
            }
            set
            {
                GetInstance().m_lastFrameTime = value;
            }
        }

        public static void AddTime(float time)
        {
            GetInstance().m_currentTime += time;
        }

        public static int GetSeconds()
        {
            return (int)GetInstance().m_currentTime;
        }
        
        public static int GetMilliseconds()
        {
            return (int)(GetInstance().m_currentTime * 1000);
        }
    }
}
