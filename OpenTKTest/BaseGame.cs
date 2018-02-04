using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTKTest.IO;
using OpenTKTest.Render;
using OpenTKTest.Game;

namespace OpenTKTest
{
    class BaseGame : OpenTK.GameWindow
    {
        int frameCap = 244;
        DateTime lastFrameCollection;
        int framesLastSecond;
        World world;

        #region "Game properties"
        const string gameName = "Guns and Fries";
        const string gameVersion = "0";
        const string windowTitle = "%{gamename} %{gamever} - ogl %{glver} %{glvendor} (%{timef} flicks, %{times} seconds, %{timems} milliseconds)";
        #endregion

        public BaseGame() : base(1280, 720, new GraphicsMode(ColorFormat.Empty, 32), "WINDOW", GameWindowFlags.Default, DisplayDevice.Default, 4, 6, GraphicsContextFlags.Default)
        {
            UpdateWindowTitle();
            lastFrameCollection = DateTime.Now;
            Blocks.LoadBlocksFromFolder("Content\\Blocks");
            FileCache.AddTexture("Content\\Blocks\\guns and fries.png", Texture.LoadFromFile("Content\\Blocks\\guns and fries.png"));
            world = new World();
            world.Init();
            DiscordController.Init();
            Analytics.CreateInstance();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(Size);
            base.OnResize(e);
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GC.Collect(2, GCCollectionMode.Optimized, false);

            TimeController.AddTime((float)e.Time);
            DateTime frameStart = DateTime.Now;
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearDepth(1);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);

            world.Render();

            BaseRenderer.RenderQuad(new Vector2(500,500), new Vector2(128, 128), "Content\\Blocks\\guns and fries.png");

            this.SwapBuffers();

            #region "Framerate logic"
            DateTime frameEnd = DateTime.Now;
            if (frameCap > 0 && (frameEnd - frameStart).TotalMilliseconds < 1000.0f/frameCap)
                System.Threading.Thread.Sleep((int)((1000.0f/frameCap) - (frameEnd - frameStart).TotalMilliseconds));
            ++framesLastSecond;

            if ((DateTime.Now - lastFrameCollection).TotalMilliseconds >= 1000)
            {
                UpdateWindowTitle();
                //Console.WriteLine(framesLastSecond + "FPS");
                framesLastSecond = 0;
                lastFrameCollection = DateTime.Now;
            }
            #endregion
        }

        protected void UpdateWindowTitle()
        {
            DiscordController.Update();
            this.Title = windowTitle.Replace("%{gamename}", gameName)
                .Replace("%{gamever}", gameVersion)
                .Replace("%{glver}", GL.GetString(StringName.Version))
                .Replace("%{glslver}", GL.GetString(StringName.ShadingLanguageVersion))
                .Replace("%{glvendor}", GL.GetString(StringName.Vendor))
                .Replace("%{timef}", TimeController.GetFlicks().ToString())
                .Replace("%{times}", TimeController.GetSeconds().ToString())
                .Replace("%{timems}", TimeController.GetMilliseconds().ToString()); // formatted window title
        }
    }
}
