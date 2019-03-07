using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Render;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.IO
{
    public enum MouseLockMode
    {
        None,
        Center
    }
    [NeedsRefactoring]
    public class InputStatus
    {
        private InputStatus m_statusLastTick;
        public InputStatus statusLastTick { get { if (m_statusLastTick == null) return new InputStatus(); return m_statusLastTick; } set => m_statusLastTick = value; }
        public Vector2 mousePosition = Vector2.Zero;
        public bool mouseButtonLeft = false;
        public bool mouseButtonRight = false;
        public Vector2 mouseDelta = Vector2.Zero;
        public List<Key> keyboardKeysDown = new List<Key>();
        public Dictionary<Key, bool> keyboardKeys = new Dictionary<Key, bool>();
        public bool shift = false;

        public InputStatus() { }
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
        public delegate void MouseEventHandler(object sender, MouseEventArgs e);
        public delegate void MousePressedEventHandler(object sender, MouseButtonEventArgs e);

        public MouseEventHandler onMouseMoved;
        public MousePressedEventHandler onMousePressed;
        public KeyEventHandler onKeyPressed;

        public MouseLockMode mouseLockMode = MouseLockMode.None; // TODO

        public static InputStatus GetStatus()
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                return instance.currentStatus = new InputStatus();
            return instance.currentStatus;
        }

        public bool GetKey(Key key)
        {
            return (currentStatus.keyboardKeys.ContainsKey(key) && currentStatus.keyboardKeys[key]);
        }

        internal static void SetLastFrameStatus(InputStatus statusLastTick)
        {
            GetInstance().currentStatus.statusLastTick = statusLastTick;
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
            var newMousePos = new Vector2(mouseX, mouseY);
            instance.currentStatus.mouseDelta = newMousePos - instance.currentStatus.mousePosition;
            instance.currentStatus.mousePosition = newMousePos;
            // HACK: this won't work on setups that have different monitor sizes, different monitor layouts, etc.
            // HACK: fix this!
            if (instance.mouseLockMode == MouseLockMode.Center) Mouse.SetPosition(((GlobalSettings.Default.resolutionX * GlobalSettings.Default.monitor)) + (GlobalSettings.Default.resolutionX / 2), GlobalSettings.Default.resolutionY / 2);
            if (Math.Ceiling(instance.currentStatus.mouseDelta.Length) != 0)
                instance.onMouseMoved?.Invoke(null, new MouseEventArgs((int)instance.currentStatus.mouseDelta.X, (int)instance.currentStatus.mouseDelta.Y));
        }

        internal static void UpdateMouseLeft(bool pressed)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mouseButtonLeft = pressed;
            instance.onMousePressed?.Invoke(null, new MouseButtonEventArgs(0, 0, MouseButton.Left, pressed));
        }

        internal static void UpdateMouseRight(bool pressed)
        {
            var instance = GetInstance();
            if (instance == null || instance.currentStatus == null)
                instance.currentStatus = new InputStatus();
            instance.currentStatus.mouseButtonRight = pressed;
            instance.onMousePressed?.Invoke(null, new MouseButtonEventArgs(0, 0, MouseButton.Right, pressed));
        }
    }
}
