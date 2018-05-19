using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whirlpool.Core;
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

        Model model;

        public void Init()
        {
            netClient = new Network.Client();
            //networkThread = new Thread(() => netClient.Update());
            //networkThread.Start();

            localPlayer = new LocalPlayer();
            localPlayer.Init();

            netClient.OnUpdate = () =>
            {
                byte[] playerPos = new byte[3];
                playerPos[0] = (byte)localPlayer.position.X;
                playerPos[1] = (byte)localPlayer.position.Y;
                playerPos[2] = (byte)localPlayer.position.Z;
                netClient.Send(Network.PacketType.Move, playerPos);
            };

            model = new Model() { objName = "Content\\octahedron.obj" };
            model.Init(null);
        }

        public void Update()
        {
            netClient.Update();
            model.Update();
        }

        public void Render()
        {
            model.Render();
        }
    }
}
