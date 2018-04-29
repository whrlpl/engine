using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Whirlpool.NetClientTest
{
    class Program
    {
        static TcpClient client;
        static void Main(string[] args)
        {
            client = new TcpClient("127.0.0.1", 15535);
            while (true)
            {
                Console.Clear();
                var str = client.GetStream();
                byte[] buf = new byte[1024];
                str.Read(buf, 0, 1024);

                Console.WriteLine(Encoding.ASCII.GetString(buf));
            }
        }
    }
}
