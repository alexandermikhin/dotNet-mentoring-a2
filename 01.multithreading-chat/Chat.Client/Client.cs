using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat.Client
{
    class Client
    {
        readonly Random random = new Random();
        readonly int minDelay = 100;
        readonly int maxDelay = 3000;
        readonly int port = 12000;
        readonly string address = "127.0.0.1";
        TcpClient client;
        NetworkStream stream;
        readonly string userName;
        readonly List<string> messages = new List<string>();

        public Client(string userName)
        {
            this.userName = userName;
            InitMessages();
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
            try
            {
                client = new TcpClient(address, port);
                stream = client.GetStream();
                WriteMessage(userName);
                var readTask = Task.Factory.StartNew(() => ReadTask(token), token);
                var writeTask = Task.Factory.StartNew(() => WriteTask(token), token);
                Task.WaitAll(readTask, writeTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured " + ex);
            }
            finally
            {
                Disconnect();
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

        private void ReadTask(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    byte[] data = new byte[256];
                    var builder = new StringBuilder();
                    while (stream.DataAvailable)
                    {
                        int bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        Console.WriteLine(builder.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during read " + ex.Message);
            }
        }

        private void WriteTask(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var delay = random.Next(minDelay, maxDelay);
                    Thread.Sleep(delay);
                    var message = GetMessage();
                    WriteMessage(message);
                    Console.WriteLine(message.PadLeft(80, ' '));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during write " + ex.Message);
            }
        }

        private void InitMessages()
        {
            for (var i = 0; i < 100; i++)
            {
                messages.Add($"Message {i}.");
            }
        }

        private string GetMessage()
        {
            var index = random.Next(0, messages.Count);

            return messages[index];
        }

        private void WriteMessage(string message)
        {
            var data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void Disconnect()
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
