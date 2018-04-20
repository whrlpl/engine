using OpenTK;
using Whirlpool.Core.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.Core.IO
{
    public class InputStatus
    {
        public Vector2 mousePosition;
        public bool mouseButtonLeft;
        public bool mouseButtonRight;
        public char[] keyboardKeysDown;
        public char[] keyboardKeysUp;
    }

    public class InputHandler : Singleton<InputHandler>
    {
        public InputStatus GetStatus()
        {
            return new InputStatus()
            {
                mousePosition = new Vector2(0, 0),
                mouseButtonLeft = true,
                mouseButtonRight = true,
                keyboardKeysDown = new[]
                {
                    'a', 'b', 'y', 'z'
                }
            };

        }
    }
}
