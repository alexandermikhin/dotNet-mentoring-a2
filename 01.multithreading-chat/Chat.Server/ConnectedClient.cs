using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Chat.Shared;
using System.Threading;

namespace Chat.Server
{
    class ConnectedClient
    {
        readonly TcpClient client;
        readonly MessagesRepository messagesRepository;
        readonly Random random = new Random();
        readonly int minDelay = 500;
        readonly int maxDelay = 1000;

        public ConnectedClient(TcpClient client, MessagesRepository messagesRepository)
        {
            this.client = client;
            this.messagesRepository = messagesRepository;
        }

        public void Process()
        {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                while (true)
                {
                    byte[] data = new byte[256];
                    var builder = new StringBuilder();
                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    var delay = random.Next(minDelay, maxDelay);
                    Thread.Sleep(delay);
                    var message = messagesRepository.GetMessage();
                    data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
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
        }
    }
}
