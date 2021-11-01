using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Client
{
    class Client
    {
        readonly Random random = new Random();
        readonly int minDelay = 1000;
        readonly int maxDelay = 3000;
        readonly int port = 12000;
        readonly string address = "127.0.0.1";
        TcpClient client;
        NetworkStream stream;
        readonly string userName;
        readonly List<string> messages = new List<string>();
        object serverStoppedLock = new object();
        bool serverStoppedMessageDisplayed = false;

        public Client(string userName)
        {
            this.userName = userName;
            InitMessages();
        }

        public void Start()
        {
            Chat();
            Console.WriteLine("Finished. Press any key");
            Console.ReadKey();
        }

        private void Chat()
        {
            try
            {
                client = new TcpClient(address, port);
                stream = client.GetStream();
                WriteMessage(userName);
                //GetChatHistory();
                var cancellationTokenSource = new CancellationTokenSource();
                var cancelTask = Task.Factory.StartNew(() => CancelTask(cancellationTokenSource));
                var readTask = Task.Factory.StartNew(() => ReadTask(cancellationTokenSource.Token), cancellationTokenSource.Token);
                var writeTask = Task.Factory.StartNew(() => WriteTask(cancellationTokenSource.Token), cancellationTokenSource.Token);
                Task.WaitAll(readTask, writeTask);
            }
            catch (SocketException)
            {
                Console.WriteLine("Could not find server. Exiting.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured " + ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        private void CancelTask(CancellationTokenSource source)
        {
            Console.WriteLine("Press 'x' to exit chat.");
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
                    var message = ReadMessage();
                    Console.WriteLine(message);
                }
            }
            catch (IOException)
            {
                HandleServerStoppedException();
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
            catch (IOException)
            {
                HandleServerStoppedException();
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
                messages.Add($"Message {i} from {userName}.");
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

        private void GetChatHistory()
        {
            Console.WriteLine("Chat history");
            Console.WriteLine(ReadMessage());
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

        private void HandleServerStoppedException()
        {
            lock (serverStoppedLock)
            {
                if (!serverStoppedMessageDisplayed)
                {
                    Console.WriteLine("Server stopped. Exiting");
                    serverStoppedMessageDisplayed = true;
                }
            }
        }
    }
}
