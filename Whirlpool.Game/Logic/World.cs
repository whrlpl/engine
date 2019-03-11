using OpenTK.Graphics;
using System;
using System.Threading;
using Whirlpool.Core.IO;
using Whirlpool.Core.Render;
using Whirlpool.Game.Render;
using Network = Whirlpool.Core.Network;

namespace Whirlpool.Game.Logic
{
    public class World
    {
        // networking
        public Network.Client netClient;
        public Thread networkThread; // networking runs on an alternative thread for speed purposes
        public LocalPlayer localPlayer;

        // perlin noise test
        public Texture perlinTest;

        public Model model, terrain;

        public void Init()
        {
            netClient = new Network.Client();
            //networkThread = new Thread(() => netClient.Update());
            //networkThread.Start();

            localPlayer = new LocalPlayer();
            localPlayer.Init();
            /*
             *        |+y  /-z   
             *        |   /    
             *        |  /    
             *        | /     
             *        |/       
             * -x--------------+x
             *       /|        
             *      / |        
             *     /  |        
             *    /   |        
             *   /    |-y        
             */
            localPlayer.position = new OpenTK.Vector3(0.0f, 0.0f, -1.0f);

            netClient.OnUpdate = () =>
            {
                byte[] playerPos = new byte[3];
                playerPos[0] = (byte)localPlayer.position.X;
                playerPos[1] = (byte)localPlayer.position.Y;
                playerPos[2] = (byte)localPlayer.position.Z;
                netClient.Send(Network.PacketType.Move, playerPos);
            };


            model = new Model() { objName = "Models\\Hagrid.obj", rotation = new OpenTK.Vector3(0.0f, 270.0f, 0.0f) };
            terrain = new Model() { objName = "Models\\floor.obj" };
            model.Init();
            terrain.Init();
            // We managed to connect to the server, lets update the discord presence with server data
            DiscordController.currentPresence = new UnsafeNativeMethods.RichPresence()
            {
                details = "In game",
                state = "Idle",
                partySize = 1,
                partyMax = 128,
                partyId = "partyid325891",
                joinSecret = "joinsecret23858",
                largeImageKey = "ingame"
            };

            perlinTest = new PerlinNoise().generateTexture();
        }

        public void Update()
        {
            if (localPlayer == null) return; // not inited before first update
            localPlayer.Update();
            netClient.Update();
            model.position = localPlayer.position;
            //localPlayer.camera.position = new OpenTK.Vector3(localPlayer.position.X, localPlayer.position.Y + 1.0f, localPlayer.position.Z);
            model.Update();
            terrain.Update();

            //debugText = "";

            //string[] variablesToDebug = new[] { "model.position", "terrain.position", "model.rotation", "terrain.rotation", "localPlayer.camera.position", "localPlayer.camera.hAngle", "localPlayer.camera.vAngle" };

            //foreach (string variable in variablesToDebug)
            //{
            //    var levels = variable.Split('.');
            //    var fieldInfo = GetType().GetField(levels[0]).GetValue(this);
            //    for (int i = 1; i < levels.Length; ++i) fieldInfo = fieldInfo.GetType().GetField(levels[i]).GetValue(fieldInfo);
            //    debugText += variable + " = " + fieldInfo.ToString() + "\n";
            //}

            //debugTextScreen.GetUIComponent("DebugText").text = debugText;
            //debugTextScreen.Update();
        }

        public void Render()
        {
            model.Render();
            terrain.Render();
            Renderer.RenderQuad(new OpenTK.Vector2(100, 100), new OpenTK.Vector2(128, 128), perlinTest, Color4.White);
        }
    }
}
