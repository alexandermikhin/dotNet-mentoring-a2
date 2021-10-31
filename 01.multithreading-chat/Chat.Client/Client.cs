using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Chat.Shared;

namespace Chat.Client
{
    class Client
    {
        readonly MessagesRepository messagesRepository;
        readonly Random random = new Random();
        readonly int minDelay = 500;
        readonly int maxDelay = 1000;
        readonly int port = 12000;
        readonly string address = "127.0.0.1";

        public Client(MessagesRepository messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public void Start()
        {
            Console.WriteLine("Press 'x' to exit chat.");
            var cancellationTokenSource = new CancellationTokenSource();
            var cancelTask = Task.Factory.StartNew(() => CancelTask(cancellationTokenSource));
            var chat = Task.Factory.StartNew(() => Chat(cancellationTokenSource.Token), cancellationTokenSource.Token);
            chat.Wait();
            Console.WriteLine("Finished. Press any key");
            Console.ReadKey();
        }

        private void Chat(CancellationToken token)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient(address, port);
                NetworkStream stream = client.GetStream();
                while (!token.IsCancellationRequested)
                {
                    var delay = random.Next(minDelay, maxDelay);
                    Thread.Sleep(delay);
                    var message = messagesRepository.GetMessage();
                    Console.WriteLine(message);
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    data = new byte[256];
                    var builder = new StringBuilder();

                    do
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    message = builder.ToString() + " <";
                    Console.WriteLine(message.PadLeft(80, ' '));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured " + ex);
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        private void CancelTask(CancellationTokenSource source)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey();
            }
            while (keyInfo.KeyChar != 'x');
            source.Cancel();
        }
    }
}
