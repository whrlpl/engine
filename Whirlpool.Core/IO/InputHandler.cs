using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
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
        public Dictionary<Key, bool> keyboardKeys = new Dictionary<Key, bool>();
        public bool shift = false;
    }

    public class KeyEventArgs : EventArgs
    {
        public Key key;
        public bool pressed;
        public KeyEventArgs(Key key, bool pressed)
        {
            this.key = key;
            this.pressed = pressed;
        }
    }

    [NeedsRefactoring]
    public class InputHandler : Singleton<InputHandler>
    {
        public InputStatus currentStatus;
        public delegate void KeyEventHandler(object sender, KeyEventArgs e);
        public KeyEventHandler onMousePressed;
        public KeyEventHandler onKeyPressed;

        public static InputStatus GetStatus()
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                return instance.currentStatus = new InputStatus();
            return instance.currentStatus;
        }

        internal static void SetKeyboardKey(Key key, bool state)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            if (key == Key.ShiftLeft || key == Key.ShiftRight)
                instance.currentStatus.shift = state; // BUG: if one shift is held down and the other is released, it will register as no shift
            if (instance.currentStatus.keyboardKeys.ContainsKey(key))
                instance.currentStatus.keyboardKeys[key] = state;
            else
                instance.currentStatus.keyboardKeys.Add(key, state);
            if (state)
                instance.onKeyPressed?.Invoke(null, new KeyEventArgs(key, state));
        }

        internal static void UpdateMousePos(int mouseX, int mouseY)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mousePosition = new Vector2(mouseX, mouseY + 48); // BUG: theres an offset of about 48 pixels.  HACK: we should figure out why this happens
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
