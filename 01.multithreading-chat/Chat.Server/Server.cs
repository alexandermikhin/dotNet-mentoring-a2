using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat.Server
{
    class Server
    {
        readonly int port = 12000;
        readonly string host = "127.0.0.1";
        readonly List<ConnectedClient> clients = new List<ConnectedClient>();

        TcpListener listener;

        public void Start()
        {
            try
            {
                IPAddress address = IPAddress.Parse(host);
                listener = new TcpListener(address, port);
                Console.WriteLine("Start server");
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ConnectedClient connectedClient = new ConnectedClient(client, this);
                    Task.Factory.StartNew(() => connectedClient.Process());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex);
                StopListener();
                CloseClients();
            }
        }

        public void AddClient(ConnectedClient client)
        {
            this.clients.Add(client);
        }

        public void IntroduceUser(ConnectedClient client)
        {
            var message = client.UserName + " entered the chart";
            BroadcastMessage(message, client.Id);
        }

        public void UserLeftChat(ConnectedClient client)
        {
            var message = client.UserName + " left the chat";
            BroadcastMessage(message, client.Id);
        }

        public void BroadcastMessage(string message, Guid id)
        {
            foreach (var client in clients)
            {
                if (client.Id != id)
                {
                    client.WriteMessage(message);
                }
            }
        }

        public void RemoveClient(ConnectedClient client)
        {
            clients.Remove(client);
        }

        public void Disconnect()
        {
            StopListener();
            CloseClients();
        }

        private void StopListener()
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }

        private void CloseClients()
        {
            clients.ForEach(c => c.Close());
        }
    }
}
