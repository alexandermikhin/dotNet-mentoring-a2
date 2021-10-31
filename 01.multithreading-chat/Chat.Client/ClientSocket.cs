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
    class ClientSocket
    {
        readonly MessagesRepository messagesRepository;
        readonly Random random = new Random();
        readonly int minDelay = 500;
        readonly int maxDelay = 1000;
        readonly int port = 12000;

        public ClientSocket(MessagesRepository messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public void Start()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancelTask = Task.Factory.StartNew(() => CancelTask(cancellationTokenSource));
            var chat = Task.Factory.StartNew(() => Chat(cancellationTokenSource.Token), cancellationTokenSource.Token);
            chat.Wait();
        }

        private void Chat(CancellationToken token)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(address, port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Console.WriteLine("Press 'x' to exit chat.");
                socket.Connect(ipEndPoint);
                while (!token.IsCancellationRequested)
                {
                    var delay = random.Next(minDelay, maxDelay);
                    Thread.Sleep(delay);
                    var message = messagesRepository.GetMessage();
                    Console.WriteLine("> " + message);
                    var data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);
                    data = new byte[256];
                    var builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, SocketFlags.None);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                }
                // socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured " + ex);
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
