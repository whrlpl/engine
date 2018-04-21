using OpenTK;
using Whirlpool.Core.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

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
        public InputStatus currentStatus;

        public EventHandler onMousePressed;

        public static InputStatus GetStatus()
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                return instance.currentStatus = new InputStatus();
            return instance.currentStatus;
        }

        public static void UpdateMousePos(int mouseX, int mouseY)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mousePosition = new Vector2(mouseX, mouseY);
        }

        public static void UpdateMouseLeft(bool pressed)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mouseButtonLeft = pressed;
            instance.onMousePressed(null, null);
        }

        public static void UpdateMouseRight(bool pressed)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mouseButtonRight = pressed;
        }
    }
}
