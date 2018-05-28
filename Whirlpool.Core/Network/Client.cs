using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Whirlpool.Core.IO;
using Whirlpool.Core.Type;

namespace Whirlpool.Core.Network
{
    [NeedsRefactoring]
    public class CustomUdpClient : UdpClient
    {
        public new bool Active;
    }

    [NeedsRefactoring]
    public enum PacketType : byte
    {
        Update = 0x00,

        Move = 0x01,

        Handshake = 0xFF,
    }

    [NeedsRefactoring]
    public class Client
    {
        // basic network client
        // this isn't the final thing!!!
        CustomUdpClient client;
        IPEndPoint endPoint;
        bool connected = false;

        public delegate void UpdateFunc();
        public UpdateFunc OnUpdate;

        public Client()
        {
            client = new CustomUdpClient();
            endPoint = new IPEndPoint(IPAddress.Loopback, 15535);
            client.Connect(endPoint);
            client.Send(new byte[] { (byte)PacketType.Handshake }, 1); // handshake
            if (client.Active)
            {
                connected = true;
            }
            else
                Logging.Write("Could not connect to server.", LogStatus.Error);
        }

        public void Send(PacketType packetType, byte[] data)
        {
            if (data == null)
            {
                client.Send(new byte[] { (byte)packetType }, 1);
            }
            else
            {
                byte[] temp = new byte[data.Length + 1];
                for (int i = 0; i < data.Length; ++i)
                    temp[i + 1] = data[i];
                temp[0] = (byte)packetType;
                client.Send(temp, 1);
            }
        }

        public void Update()
        {
            if (!connected) return;
            byte[] pckt = client.Receive(ref endPoint);
            if (pckt[0] == (byte)PacketType.Update) // received raw packet
                Logging.Write("Recieved packet: " + Encoding.ASCII.GetString(pckt));
            OnUpdate();
        }
    }
}
