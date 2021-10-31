using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat.Server
{
    class ServerSocket
    {
        readonly int port = 12000;
        readonly int backlog = 10;

        public void Start()
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(address, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Console.WriteLine("Start server");
                socket.Bind(ipEndPoint);
                socket.Listen(backlog);
                
                while (true)
                {
                    var handler = socket.Accept();
                    var builder = new StringBuilder();
                    int bytes = 0;
                    var data = new byte[256];

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                    string message = "message delivered";
                    data = Encoding.Unicode.GetBytes(message);
                    Console.WriteLine("Handler send");
                    handler.Send(data);
                }
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex);
            }
        }
    }
}
