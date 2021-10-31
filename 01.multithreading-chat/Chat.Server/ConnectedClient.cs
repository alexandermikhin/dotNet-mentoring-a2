using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Chat.Server
{
    class ConnectedClient
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }

        readonly TcpClient client;
        readonly Server server;
        NetworkStream stream;

        public ConnectedClient(TcpClient client, Server server)
        {
            Id = Guid.NewGuid();
            this.client = client;
            this.server = server;
            this.server.AddClient(this);
        }

        public void Process()
        {
            try
            {
                stream = client.GetStream();
                UserName = ReadMessage();
                server.IntroduceUser(this);
                //ShareLatestMessagesHistory();
                ReadMessages();
            }
            catch (Exception ex)
            {
                server.RemoveClient(this);
                server.UserLeftChat(this);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();
            }
        }

        private void ReadMessages()
        {
            //try
            //{
                while (true)
                {
                    var message = ReadMessage();
                    server.UpdateMessageHistory(this, message);
                    message = UserName + ": " + message;
                    server.BroadcastMessage(message, this);
                }
            //}
            //catch (Exception ex)
            //{
            //    server.UserLeftChat(this);
            //    Console.WriteLine("Exception during read " + ex.Message);
            //}
        }

        private string ReadMessage()
        {
            byte[] data = new byte[256];
            var builder = new StringBuilder();
            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            return builder.ToString();
        }

        public void WriteMessage(string message)
        {
            try
            {
                var data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server exception during write" + ex.Message);
            }
        }

        public void Close()
        {
            if (stream != null)
            {
                stream.Close();
            }

            if (client != null)
            {
                client.Close();
            }
        }

        private void ShareLatestMessagesHistory()
        {
            var messages = server.GetMessagesHistory();
            var builder = new StringBuilder();
            foreach (var message in messages)
            {
                builder.Append(message.Item1 + ": " + message.Item2 + Environment.NewLine);
            }

            WriteMessage(builder.ToString());
        }
    }
}
