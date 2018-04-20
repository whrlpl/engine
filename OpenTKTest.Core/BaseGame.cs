using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTKTest.Core.IO;
using OpenTKTest.Core.Render;
using OpenTKTest.Bytecode.Interpreter;
using OpenTK.Input;

namespace OpenTKTest.Core
{
    public abstract class BaseGame : OpenTK.GameWindow
    {
        int frameCap = -1;
        DateTime lastFrameCollection;
        int framesLastSecond;

        public VM gameBytecodeVM;

        #region "Game properties"
        public string gameName = "Guns and Fries";
        public string gameVersion = "0";
        public string windowTitle = "%{gamename} %{gamever} | ogl %{glver} | %{fps} fps";
        #endregion

        public delegate void actionDelegate();
        public abstract void Initialize();
        public abstract void Render();
        public abstract void Update();

        public BaseGame() : base(1280, 720, new GraphicsMode(ColorFormat.Empty, 32), "Window", GameWindowFlags.Default, DisplayDevice.Default, 4, 6, GraphicsContextFlags.Default)
        {
            UpdateWindowTitle();
            lastFrameCollection = DateTime.Now;
            FileCache.AddTexture("blank", Texture.FromData(new Color4[] { Color4.White }, 1, 1));
            FileCache.LoadTexturesFromFolder("Content");
            DiscordController.Init();
            Analytics.CreateInstance();

            gameBytecodeVM = new VM();
            //gameBytecodeVM.RunFile("Content\\Game\\main.abc");

            Initialize();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(Size);
            BaseRenderer.GetInstance().windowSize = new Vector2(Size.Width, Size.Height);
            base.OnResize(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
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
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            BaseRenderer.GetInstance().camera?.Update();
            Render();

            this.SwapBuffers();

            #region "Framerate logic"
            DateTime frameEnd = DateTime.Now;
            if (frameCap > 0 && (frameEnd - frameStart).TotalMilliseconds < 1000.0f/frameCap)
                System.Threading.Thread.Sleep((int)((1000.0f/frameCap) - (frameEnd - frameStart).TotalMilliseconds));
            ++framesLastSecond;

            if ((DateTime.Now - lastFrameCollection).TotalMilliseconds >= 1000)
            {
                UpdateWindowTitle();
                Console.WriteLine(framesLastSecond + "FPS");
                framesLastSecond = 0;
                lastFrameCollection = DateTime.Now;
            }
            #endregion
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Time.GetMilliseconds() % 20 == 0)
            {
                //InputHandler.Update();
                DiscordController.Update();
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