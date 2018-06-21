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

        Model model;
        Model terrain;

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


            model = new Model() { objName = "Content\\Hagrid.obj", rotation = new OpenTK.Vector3(0.0f, 270.0f, 0.0f) };
            terrain = new Model() { objName = "Content\\floor.obj" };
            model.Init(null);
            terrain.Init(null);
            MainGame.currentScreens[0].GetUIComponent("TestLabel").text = "*Test* test";
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
            localPlayer.Update();
            netClient.Update();
            model.position = localPlayer.position;
            localPlayer.camera.position = new OpenTK.Vector3(localPlayer.position.X, localPlayer.position.Y + 1.0f, localPlayer.position.Z);
            model.Update();
            terrain.Update();
        }

        public void Render()
        {
            model.Render();
            terrain.Render();
            Renderer.RenderQuad(new OpenTK.Vector2(100, 100), new OpenTK.Vector2(128, 128), perlinTest, Color4.White);
        }
    }
}
