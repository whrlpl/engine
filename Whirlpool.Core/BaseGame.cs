using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;
using Whirlpool.Core.Render.Nova;

namespace Whirlpool.Core
{
    public abstract class BaseGame : OpenTK.GameWindow
    {
        int frameCap = 30;
        DateTime lastFrameCollection;
        public float framesLastSecond;
        public Thread updateThread;

        public bool initialized;

        public static new System.Drawing.Size Size = new System.Drawing.Size(GlobalSettings.Default.resolutionX, GlobalSettings.Default.resolutionY);

        #region "Game properties"
        public static string gameName { get; set; }
        public static string gameVersion { get; set; }
        public static string windowTitle { get; set; }
#endregion

        public abstract void Render();
        public abstract void Init();
        public abstract void Update();
        public abstract void OneSecondPassed();

        public BaseGame() : base(
            Size.Width,
            Size.Height,
            GraphicsMode.Default,
            windowTitle,
            GlobalSettings.Default.fullscreenMode ? GameWindowFlags.Fullscreen : GameWindowFlags.FixedWindow,
            DisplayDevice.Default,
            4, 6,
            GraphicsContextFlags.ForwardCompatible) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            updateThread = new Thread(UpdateThread);
            updateThread.Start();

            FileBank.AddTexture("blank", Texture.FromData(new Color4[] { Color4.White }, 1, 1));
            FileBank.LoadTexturesFromFolder("Content");

            DiscordController.Init();
            Mouse.ButtonDown += Mouse_ButtonDown;
            Mouse.ButtonUp += Mouse_ButtonUp;

            lastFrameCollection = DateTime.Now;
            Init();
            Render3D.Init();
            Render2D.Init();
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
            base.OnResize(e);
            GL.Viewport(Size);
            Renderer.GetInstance().windowSize = new Vector2(Size.Width, Size.Height);
            //PostProcessing.GetInstance().Resize(Size);
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
            base.OnRenderFrame(e);
            Time.AddTime((float)e.Time);
            DateTime frameStart = DateTime.Now;

            PostProcessing.GetInstance().PreRender();

            Render();
            
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
            initialized = true; // everything is only fully initialized after first render call.
        }

        protected void UpdateThread()
        {
            while (true)
            {
                if (!initialized) continue;
                InputHandler.UpdateMousePos(Mouse.X, Mouse.Y);

                if (Time.GetMilliseconds() % 15000 == 0)
                {
                    DiscordController.Update();
                }

                Update();
            }
        }
    }
}