using System;
using OpenTK;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.IO
{
    [NeedsRefactoring]
    public class InputStatus
    {
        public Vector2 mousePosition = Vector2.Zero;
        public bool mouseButtonLeft = false;
        public bool mouseButtonRight = false;
        public bool[] keyboardKeys = new bool[512];
    }


    [NeedsRefactoring]
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

        internal static void SetKeyboardKey(char key, bool state)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.keyboardKeys[key] = state;
        }

        internal static void UpdateMousePos(int mouseX, int mouseY)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mousePosition = new Vector2(mouseX, mouseY);
        }

        internal static void UpdateMouseLeft(bool pressed)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mouseButtonLeft = pressed;
            instance.onMousePressed?.Invoke(null, null);
        }

        internal static void UpdateMouseRight(bool pressed)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mouseButtonRight = pressed;
        }
    }
}
