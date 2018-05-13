using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace Whirlpool.Server
{
    // really basic server setup
    // sends chat log every second
    // very lazy and follows no set structure or pattern.  enjoy as you look at the shittiest code i could write at 1 am

    class Vector3 // basic vector3 implementation so we dont have to rely on opentk
    {
        float X; float Y; float Z;
        public Vector3(float X, float Y, float Z) { this.X = X; this.Y = Y; this.Z = Z; }
    }

    class RemotePlayer
    {
        public IPEndPoint endPoint;
        public Vector3 position;
        public Vector3 rotation;
    }

    class Program
    {
        const int THREADS = 2;
        const int DATA_SIZE = 1024;
        const int TICK = 1000 / 20; // 20tps
        static List<RemotePlayer>[] clientList = new List<RemotePlayer>[THREADS];
        static Thread[] threads = new Thread[THREADS];
        static byte[] writeData = new byte[DATA_SIZE];

        static UdpClient udpListener;

        static bool shouldExit = false;

        static void ClientThread(object thread)
        {
            while (!shouldExit)
            {
                var startTime = DateTime.Now;
                List<RemotePlayer>[] clientListTmp = new List<RemotePlayer>[THREADS];
                clientList.CopyTo(clientListTmp, 0);
                foreach (var client in clientListTmp[(int)thread])
                {
                    udpListener.Send(writeData, 1024, client.endPoint);
                }
                var endTime = DateTime.Now;
                Thread.Sleep(TICK - (int)(endTime-startTime).TotalMilliseconds);
            }
        }

        static void Main(string[] args)
        {
            udpListener = new UdpClient(15535);
            // initialize client threads
            for (var i = 0; i < THREADS; ++i)
            {
                clientList[i] = new List<RemotePlayer>();
                threads[i] = new Thread(ClientThread);
                threads[i].Start(i);
            }

            Console.WriteLine("Server listening on port 15535");

            // keep track of client connections
            while (!shouldExit)
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 15535);
                var pckt = udpListener.Receive(ref endPoint);
                if (pckt[0] == 0xFF) // handshake
                    AddClient(endPoint);
                else if (pckt[0] == 0x01)
                    Console.WriteLine("move packet");

                var tempWriteData = new byte[DATA_SIZE];
                var bytes = Encoding.ASCII.GetBytes("Hello from server!");
                for (int i = 0; i < bytes.Length; ++i)
                {
                    tempWriteData[i + 1] = bytes[i];
                }
                tempWriteData[0] = 0x00;
                tempWriteData.CopyTo(writeData, 0);
            }
        }

        static void AddClient(IPEndPoint client)
        {
            // chaaange places!!!
            // add to whichever thread has the least number of clients
            var leastWorkThread = new Tuple<int, int>(0, 0);
            for (var i = 0; i < THREADS; ++i)
                if (leastWorkThread.Item1 < clientList[i].Count)
                    leastWorkThread = new Tuple<int, int>(clientList[i].Count, i);

            Console.WriteLine("client added " + client.ToString());

            clientList[leastWorkThread.Item2].Add(new RemotePlayer() {
                endPoint = client,
                position = new Vector3(0,0,0),
                rotation = new Vector3(0,0,0)
            });

        }
    }
}
