﻿using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTKTest.IO;
using OpenTKTest.Render;
using OpenTKTest.Game;

namespace OpenTKTest
{
    public abstract class BaseGame : OpenTK.GameWindow
    {
        int frameCap = -1;
        DateTime lastFrameCollection;
        int framesLastSecond;

        #region "Game properties"
        public string gameName = "Guns and Fries";
        public string gameVersion = "0";
        public string windowTitle = "%{gamename} %{gamever} | ogl %{glver} | %{fps} fps";
        #endregion

        public World world;

        public delegate void actionDelegate();
        public abstract void Initialize();
        public abstract void Render();
        public abstract void Update();

        public BaseGame() : base(1280, 720, new GraphicsMode(ColorFormat.Empty, 32), "WINDOW", GameWindowFlags.Default, DisplayDevice.Default, 4, 6, GraphicsContextFlags.Default)
        {
            UpdateWindowTitle();
            lastFrameCollection = DateTime.Now;
            Blocks.LoadBlocksFromFolder("Content\\Blocks");
            FileCache.AddTexture("blank", Texture.FromData(new Color[] { Color.White }, 1, 1));
            FileCache.LoadTexturesFromFolder("Content\\Blocks");
            world = new World();
            world.Init();
            DiscordController.Init();
            Analytics.CreateInstance();
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

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //GC.Collect(1, GCCollectionMode.Optimized, false);

            TimeController.AddTime((float)e.Time);
            DateTime frameStart = DateTime.Now;
            GL.ClearColor(Color4.CornflowerBlue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearDepth(1);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.DepthFunc(DepthFunction.Lequal);

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
                .Replace("%{timems}", TimeController.GetMilliseconds().ToString())
                .Replace("%{fps}", framesLastSecond.ToString()); // formatted window title
        }
    }
}
