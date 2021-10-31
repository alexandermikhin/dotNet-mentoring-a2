using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace Chat.Server
{
    class Server
    {
        readonly int port = 12000;
        readonly string host = "127.0.0.1";
        readonly List<ConnectedClient> clients = new List<ConnectedClient>();
        readonly ConcurrentQueue<Tuple<string, string>> latestMessages = new ConcurrentQueue<Tuple<string, string>>();
        readonly int historySize = 10;
        readonly ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

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
            readerWriterLockSlim.EnterWriteLock();
            this.clients.Add(client);
            readerWriterLockSlim.ExitWriteLock();
        }

        public void IntroduceUser(ConnectedClient client)
        {
            var message = client.UserName + " entered the chart";
            BroadcastMessage(message, client);
        }

        public void UserLeftChat(ConnectedClient client)
        {
            var message = client.UserName + " left the chat";
            BroadcastMessage(message, client);
        }

        public void BroadcastMessage(string message, ConnectedClient client)
        {
            readerWriterLockSlim.EnterReadLock();
            foreach (var c in clients)
            {
                if (client.Id != c.Id)
                {
                    c.WriteMessage(message);
                }
            }
            readerWriterLockSlim.ExitReadLock();
        }

        public void RemoveClient(ConnectedClient client)
        {
            readerWriterLockSlim.EnterWriteLock();
            clients.Remove(client);
            readerWriterLockSlim.ExitWriteLock();
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

        public void UpdateMessageHistory(ConnectedClient client, string message)
        {
            if (latestMessages.Count >= historySize)
            {
                latestMessages.TryDequeue(out _);
            }

            latestMessages.Enqueue(new Tuple<string, string>(client.UserName, message));
        }

        public IEnumerable<Tuple<string, string>> GetMessagesHistory()
        {
            return latestMessages.ToArray();
        }
    }
}
