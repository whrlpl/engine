using System;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;

namespace Whirlpool.Core
{
    public class GameProperties
    {
        public string gameName { get; set; }
        public string gameVersion { get; set; }
        public string windowTitle { get; set; }
    }
    
    public abstract class BaseGame : OpenTK.GameWindow
    {
        public static GameProperties gameProperties;
        private DateTime lastFrameCollection;
        private float lastUpdate;
        public float framesLastSecond;
        public Thread updateThread;
        public bool firstFrameRendered;
        public static new System.Drawing.Size Size = new System.Drawing.Size(GlobalSettings.Default.resolutionX, GlobalSettings.Default.resolutionY);
        public int[] frameRateGraph = new int[100];

        public abstract void Render();
        public abstract void Render2D();
        public abstract void Init();
        public abstract void Update(float deltaTime);
        public abstract void OneSecondPassed();

        public BaseGame() : base(
            Size.Width,
            Size.Height,
            GraphicsMode.Default,
            gameProperties.windowTitle,
            GlobalSettings.Default.fullscreenMode ? GameWindowFlags.Fullscreen : GameWindowFlags.FixedWindow,
            DisplayDevice.GetDisplay((DisplayIndex)GlobalSettings.Default.monitor),
            4, 6,
            GraphicsContextFlags.ForwardCompatible) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            updateThread = new Thread(UpdateThread);
            updateThread.Start();

            DiscordController.Init();
            Mouse.ButtonDown += Mouse_ButtonDown;
            Mouse.ButtonUp += Mouse_ButtonUp;

            lastFrameCollection = DateTime.Now;

            RenderShared.Init(new Vector2(GlobalSettings.Default.renderResolutionX, GlobalSettings.Default.renderResolutionY));

            Logging.Write("Gamepad info:");
            var capabilities = GamePad.GetCapabilities(0);
            if (capabilities.IsConnected)
            {
                Logging.Write(capabilities.GamePadType.ToString());
                GamePad.SetVibration(0, 1f, 1f);
            }
            else
            {
                Logging.Write("Gamepad not connected");
            }
            Init();
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
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e) => InputHandler.SetKeyboardKey(e.Key, false);
        protected override void OnKeyDown(KeyboardKeyEventArgs e) => InputHandler.SetKeyboardKey(e.Key, true);
        protected override void OnMouseMove(MouseMoveEventArgs e) => InputHandler.UpdateMousePos(e.X, e.Y);

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
            PostProcessing.GetInstance().Prepare2D();
            Render2D();
            PostProcessing.GetInstance().PostRender();

            this.SwapBuffers();

            if (GlobalSettings.Default.improvedLatency)
                GL.Finish();

            #region "Framerate logic"
            DateTime frameEnd = DateTime.Now;
            if (GlobalSettings.Default.frameCap > 0 && (frameEnd - frameStart).TotalMilliseconds < 1000.0f/GlobalSettings.Default.frameCap)
                Thread.Sleep((int)((1000.0f/GlobalSettings.Default.frameCap) - (frameEnd - frameStart).TotalMilliseconds));
            ++framesLastSecond;

            Time.lastFrameTime = (frameEnd - frameStart).TotalSeconds;

            if ((DateTime.Now - lastFrameCollection).TotalMilliseconds >= 1000)
            {
                OneSecondPassed();
                //Logging.Write(1000.0f/(frameEnd-frameStart).TotalMilliseconds + "FPS");
                for (int i = 0; i < 99; ++i)
                {
                    frameRateGraph[i + 1] = frameRateGraph[i];
                }
                frameRateGraph[99] = (int)(1000.0f / (frameEnd - frameStart).TotalMilliseconds);
                framesLastSecond = 0;
                lastFrameCollection = DateTime.Now;
            }
            #endregion
            if (firstFrameRendered) return;
            firstFrameRendered = true;
        }

        protected void HandleInput()
        {
            InputStatus lastInputStatus = InputHandler.GetStatus();
            InputHandler.SetLastFrameStatus(lastInputStatus);
        }

        protected void UpdateThread()
        {
            while (true)
            {
                if (!firstFrameRendered) continue;
                if (Time.currentTime * 1000 - lastUpdate < 1) continue;

                HandleInput();

                if (Time.currentTime * 1000 % 15000 == 0) DiscordController.Update();

                Update(1000 / (Time.GetMilliseconds() - lastUpdate));
                lastUpdate = Time.GetMilliseconds();
                Thread.Sleep(1000/60);
            }
        }
    }
}