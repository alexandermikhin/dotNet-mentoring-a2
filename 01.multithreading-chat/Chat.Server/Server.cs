using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Server
{
    class Server
    {
        readonly int port = 12000;
        readonly string host = "127.0.0.1";
        readonly List<ConnectedClient> clients = new List<ConnectedClient>();
        readonly ConcurrentQueue<MessageHistoryItem> latestMessages = new ConcurrentQueue<MessageHistoryItem>();
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

            var item = new MessageHistoryItem() { UserName = client.UserName, Message = message };
            latestMessages.Enqueue(item);
        }

        public void ShareLatestMessagesHistory(ConnectedClient client)
        {
            var messages = latestMessages.ToArray();
            var builder = new StringBuilder();
            foreach (var message in messages)
            {
                builder.Append(message.UserName + ": " + message.Message);
            }

            client.WriteMessage(builder.ToString());
        }
    }
}
