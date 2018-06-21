using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;
using Whirlpool.Core.UI;
using Whirlpool.Script.Interpreter;

namespace Whirlpool.Core
{
    public abstract class BaseGame : OpenTK.GameWindow
    {
        int frameCap = -1;
        DateTime lastFrameCollection;
        public float framesLastSecond;
        public VM gameBytecodeVM;
        public Thread updateThread;
        public static List<Screen> currentScreens = new List<Screen>();
        public static Screen consoleScreen;

        public bool consoleVisible = false;

        public static new System.Drawing.Size Size = new System.Drawing.Size(GlobalSettings.Default.resolutionX, GlobalSettings.Default.resolutionY);
        public Font tempFont;

#if !DEBUG
        public string screenFile = "Content\\screens\\mainmenu.xml";
#else
        public string screenFile = "Content\\screens\\splash.xml";
#endif
        public string consoleFile = "Content\\screens\\console.xml";

        #region "Game properties"
        public static string gameName { get; set; }
        public static string gameVersion { get; set; }
        public static string windowTitle { get; set; }
#endregion

        public abstract void Render();
        public abstract void Update();
        public abstract void OneSecondPassed();

        public BaseGame() : base(
            Size.Width,
            Size.Height,
            new GraphicsMode(ColorFormat.Empty, 32), 
            windowTitle,
            GlobalSettings.Default.fullscreenMode ? GameWindowFlags.Fullscreen : GameWindowFlags.FixedWindow, 
            DisplayDevice.Default,
            4, 6, 
            GraphicsContextFlags.Default)
        {
            Init();
        }

        public virtual void Init()
        {
            updateThread = new Thread(UpdateThread);
            updateThread.Start();

            FileBank.AddTexture("blank", Texture.FromData(new Color4[] { Color4.White }, 1, 1));
            FileBank.LoadTexturesFromFolder("Content");

            currentScreens.Add(new Screen(screenFile));
            consoleScreen = new Screen(consoleFile);


            InputHandler.GetInstance().onKeyPressed += (s, e) =>
            {
                var status = InputHandler.GetStatus();
                if (status.keyboardKeys.ContainsKey(Key.F1) && status.keyboardKeys[Key.F1]) consoleVisible = !consoleVisible;
            };

            tempFont = new Font("Content\\Fonts\\Montserrat-Regular.ttf", Color4.White, 14, 0);

            DiscordController.Init();
            Mouse.ButtonDown += Mouse_ButtonDown;
            Mouse.ButtonUp += Mouse_ButtonUp;

            try 
            {
                gameBytecodeVM = new VM();
                gameBytecodeVM.RunFile("Content\\Game\\main.wcc");
            }
            catch (Exception ex)
            {
                Logging.Write("There was a problem loading the VM: " + ex.Message, LogStatus.Error);
            }

            lastFrameCollection = DateTime.Now;
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
            Renderer.GetInstance().windowSize = new Vector2(Size.Width, Size.Height);
            PostProcessing.GetInstance().Resize(Size);
            base.OnResize(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            InputHandler.SetKeyboardKey(e.Key, false);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            InputHandler.SetKeyboardKey(e.Key, true);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0);
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

            PostProcessing.GetInstance().PreRender();
            
            foreach (Screen s in currentScreens)
            {
                s.Render();
            }

            Render();

            if (consoleVisible) consoleScreen.Render();
            PostProcessing.GetInstance().PostRender();
            
            this.SwapBuffers();
            if (GlobalSettings.Default.improvedLatency)
                GL.Finish();

            #region "Framerate logic"
            DateTime frameEnd = DateTime.Now;
            if (frameCap > 0 && (frameEnd - frameStart).TotalMilliseconds < 1000.0f/frameCap)
                Thread.Sleep((int)((1000.0f/frameCap) - (frameEnd - frameStart).TotalMilliseconds));
            ++framesLastSecond;

            Time.lastFrameTime = (frameEnd - frameStart).TotalSeconds;

            if ((DateTime.Now - lastFrameCollection).TotalMilliseconds >= 1000)
            {
                OneSecondPassed();
                //Logging.Write(1000.0f/(frameEnd-frameStart).TotalMilliseconds + "FPS");
                framesLastSecond = 0;
                lastFrameCollection = DateTime.Now;
            }
#endregion
        }

        protected void UpdateThread()
        {
            while (true)
            {
                InputHandler.UpdateMousePos(Mouse.X, Mouse.Y);

                try
                {
                    foreach (Screen s in currentScreens)
                        s.Update();

                    if (consoleVisible) consoleScreen.Update();
                }
                catch (Exception ex)
                {
                    Logging.Write(ex.ToString(), LogStatus.Error);
                }

                if (Time.GetMilliseconds() % 1500 == 0)
                {
                    DiscordController.Update();
                }

                Update();
            }
        }
    }
}