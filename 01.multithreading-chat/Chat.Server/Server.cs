using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Chat.Shared;

namespace Chat.Server
{
    class Server
    {
        readonly int port = 12000;
        readonly string host = "127.0.0.1";
        readonly MessagesRepository messagesRepository;

        public Server(MessagesRepository messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public void Start()
        {
            TcpListener listener = null;
            try
            {
                IPAddress address = IPAddress.Parse(host);
                listener = new TcpListener(address, port);
                Console.WriteLine("Start server");
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ConnectedClient connectedClinet = new ConnectedClient(client, messagesRepository);
                    Task.Factory.StartNew(() => connectedClinet.Process());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }
        }
    }
}
