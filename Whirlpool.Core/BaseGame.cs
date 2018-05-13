using System;
using System.IO;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Whirlpool.Core.IO;
using Whirlpool.Core.Pattern;
using Whirlpool.Core.Render;
using Whirlpool.Core.UI;
using Whirlpool.Script.Interpreter;

namespace Whirlpool.Core
{
    public abstract class BaseGame : OpenTK.GameWindow
    {
        int frameCap = -1;
        DateTime lastFrameCollection;
        float framesLastSecond;
        public VM gameBytecodeVM;
        public Thread updateThread;
        public Screen currentScreen;
        public static new System.Drawing.Size Size = new System.Drawing.Size(1280, 720);
        public Font tempFont;
                
        public string screenFile = "Content\\screens\\splash.xml";

        private FileSystemWatcher fsWatcher;

        #region "Game properties"
        public static string gameName = "Whirlpool Engine Game";
        public static string gameVersion = "1.0";
        public static string windowTitle = "%{gamename}";
        #endregion

        public delegate void actionDelegate();
        public abstract void Render();
        public abstract void Update();

        public BaseGame() : base(
            Size.Width,
            Size.Height,
            new GraphicsMode(ColorFormat.Empty, 32), 
            windowTitle,
            GameWindowFlags.Default, 
            DisplayDevice.Default,
            4, 6, 
            GraphicsContextFlags.Default)
        {
            Init();
        }

        public void ReloadFile(object source, FileSystemEventArgs e)
        {
            currentScreen.LoadFromFile(screenFile);
        }

        public virtual void Init()
        {
            updateThread = new Thread(UpdateThread);
            updateThread.Start();
            UpdateWindowTitle();

            FileBank.AddTexture("blank", Texture.FromData(new Color4[] { Color4.White }, 1, 1));
            FileBank.LoadTexturesFromFolder("Content");

            FileSystemWatcher fsWatcher = new FileSystemWatcher();
            fsWatcher.Changed += new FileSystemEventHandler(ReloadFile);
            fsWatcher.Path = Path.GetDirectoryName(screenFile);
            fsWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fsWatcher.Filter = Path.GetFileName(screenFile);
            fsWatcher.EnableRaisingEvents = true;

            currentScreen = new Screen();
            currentScreen.AddUIComponents(UILoader.LoadFile(screenFile));
            currentScreen.Init();

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
            BaseRenderer.GetInstance().windowSize = new Vector2(Size.Width, Size.Height);
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
            Environment.Exit(0);
            base.OnClosed(e);
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
            currentScreen?.Render();
            PostProcessing.GetInstance().PostRender();
            InputStatus status = InputHandler.GetStatus();

            //new Label() { position = InputHandler.GetStatus().mousePosition, text = InputHandler.GetStatus().mousePosition.X + "," + InputHandler.GetStatus().mousePosition.Y, font = tempFont }.Render();
            this.SwapBuffers();

            #region "Framerate logic"
            DateTime frameEnd = DateTime.Now;
            if (frameCap > 0 && (frameEnd - frameStart).TotalMilliseconds < 1000.0f/frameCap)
                Thread.Sleep((int)((1000.0f/frameCap) - (frameEnd - frameStart).TotalMilliseconds));
            ++framesLastSecond;

            if ((DateTime.Now - lastFrameCollection).TotalMilliseconds >= 1000)
            {
                UpdateWindowTitle();
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
                if (Time.GetMilliseconds() % 2 == 0)
                {
                    DiscordController.Update();
                    InputHandler.UpdateMousePos(Mouse.X, Mouse.Y);
                    currentScreen?.Update();
                    Update();
                }
            }
        }

        protected void UpdateWindowTitle()
        {
            Title = windowTitle
                .Replace("%{gamename}", gameName)
                .Replace("%{gamever}", gameVersion)
                .Replace("%{glver}", GL.GetString(StringName.Version))
                .Replace("%{glslver}", GL.GetString(StringName.ShadingLanguageVersion))
                .Replace("%{glvendor}", GL.GetString(StringName.Vendor))
                .Replace("%{times}", Time.GetSeconds().ToString())
                .Replace("%{timems}", Time.GetMilliseconds().ToString())
                .Replace("%{build}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString())
                .Replace("%{fps}", framesLastSecond.ToString());
        }
    }
}