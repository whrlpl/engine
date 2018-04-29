using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;
using Whirlpool.Bytecode.Interpreter;
using OpenTK.Input;
using System.Collections.Generic;

namespace Whirlpool.Core
{
    public abstract class BaseGame : OpenTK.GameWindow
    {
        int frameCap = -1;
        DateTime lastFrameCollection;
        int framesLastSecond;
        public VM gameBytecodeVM;

        #region "Game properties"
        public static string gameName = "Whirlpool Engine Game";
        public static string gameVersion = "1.0";
        public static string windowTitle = "%{gamename}";
        #endregion

        public delegate void actionDelegate();
        public abstract void Render();
        public abstract void Update();

        public BaseGame() : base(1280, 
            720, 
            new GraphicsMode(ColorFormat.Empty, 32), 
            windowTitle, 
            GameWindowFlags.Default, 
            DisplayDevice.Default, 
            4, 6, 
            GraphicsContextFlags.Default)
        {
            Init();
        }

        public virtual void Init()
        {
            UpdateWindowTitle();
            lastFrameCollection = DateTime.Now;
            FileBank.AddTexture("blank", Texture.FromData(new Color4[] { Color4.White }, 1, 1));
            FileBank.LoadTexturesFromFolder("Content");
            DiscordController.Init();
            Mouse.ButtonDown += Mouse_ButtonDown;
            Mouse.ButtonUp += Mouse_ButtonUp;
            try
            {
                gameBytecodeVM = new VM();
                gameBytecodeVM.RunFile("Content\\Game\\main.abc");
            }
            catch
            {
                Logging.Write("There was a problem loading main.abc into the VM", LogStatus.Error);
            }
        }

        private void HandleMouseEvent(MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    InputHandler.UpdateMouseLeft(e.IsPressed);
                    break;
                case MouseButton.Right:
                    InputHandler.UpdateMouseRight(e.IsPressed);
                    break;
            }
        }

        private void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            HandleMouseEvent(e);
        }

        private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            HandleMouseEvent(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(Size);
            BaseRenderer.GetInstance().windowSize = new Vector2(Size.Width, Size.Height);
            base.OnResize(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            InputHandler.SetKeyboardKey(e.Key.ToString().ToLower()[0], false);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            InputHandler.SetKeyboardKey(e.Key.ToString().ToLower()[0], true);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Time.AddTime((float)e.Time);
            DateTime frameStart = DateTime.Now;
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearDepth(1);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);

            BaseRenderer.GetInstance().camera?.Update();

            PostProcessing.GetInstance().PreRender();

            Render();

            PostProcessing.GetInstance().PostRender();

            this.SwapBuffers();

            #region "Framerate logic"
            DateTime frameEnd = DateTime.Now;
            if (frameCap > 0 && (frameEnd - frameStart).TotalMilliseconds < 1000.0f/frameCap)
                System.Threading.Thread.Sleep((int)((1000.0f/frameCap) - (frameEnd - frameStart).TotalMilliseconds));
            ++framesLastSecond;

            if ((DateTime.Now - lastFrameCollection).TotalMilliseconds >= 1000)
            {
                UpdateWindowTitle();
                Logging.Write(framesLastSecond + "FPS");
                framesLastSecond = 0;
                lastFrameCollection = DateTime.Now;
            }
            #endregion
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Time.GetMilliseconds() % 20 == 0)
            {
                DiscordController.Update();
                InputHandler.UpdateMousePos(Mouse.X, Mouse.Y);
                Update();
            }
        }

        protected void UpdateWindowTitle()
        {
            this.Title = windowTitle.Replace("%{gamename}", gameName)
                .Replace("%{gamever}", gameVersion)
                .Replace("%{glver}", GL.GetString(StringName.Version))
                .Replace("%{glslver}", GL.GetString(StringName.ShadingLanguageVersion))
                .Replace("%{glvendor}", GL.GetString(StringName.Vendor))
                .Replace("%{timef}", Time.GetFlicks().ToString())
                .Replace("%{times}", Time.GetSeconds().ToString())
                .Replace("%{timems}", Time.GetMilliseconds().ToString())
                .Replace("%{build}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString())
                .Replace("%{fps}", framesLastSecond.ToString()); // formatted window title
        }
    }
}