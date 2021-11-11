using System;
using System.Collections.Concurrent;
using System.IO;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using DirectoryWatch;

namespace InputServer
{
    class Program
    {
        const string QueueName = @".\Private$\MessageQueue";
        const string SourceFolder = @"D:\dotNetlab\Temp\pdfs\";
        const string FileFilter = "*.pdf";
        const int ChunkSize = 10000;
        static readonly ConcurrentQueue<FileMessageBody> messagesQueue = new ConcurrentQueue<FileMessageBody>();
        static readonly AutoResetEvent messageAdded = new AutoResetEvent(false);

        static void Main(string[] args)
        {

            CreateMessageQueue();
            var watchTask = Task.Factory.StartNew(WatchDirectory);
            var serverTask = Task.Factory.StartNew(Server);
            Task.WaitAll(watchTask, serverTask);
        }

        static void CreateMessageQueue()
        {
            Console.WriteLine("Creating messsage queue");
            if (!MessageQueue.Exists(QueueName))
            {
                MessageQueue.Create(QueueName);
            }
        }

        static void Server()
        {
            while (true)
            {
                messageAdded.WaitOne();
                using (var serverQueue = new MessageQueue(QueueName))
                {
                    serverQueue.Formatter = new BinaryMessageFormatter();

                    while (!messagesQueue.IsEmpty)
                    {
                        if (messagesQueue.TryDequeue(out var message))
                        {
                            serverQueue.Send(message);
                            var consoleMessage = $"File {message.FileName} was put in queue.";
                            if (message.TotalParts > 1)
                            {
                                consoleMessage += $" Part {message.Part} of {message.TotalParts}.";
                            }
                            Console.WriteLine(consoleMessage);
                        }
                    }
                }
            }
        }

        static void WatchDirectory()
        {
            Console.WriteLine("Starting server...");
            var directoryWatcher = new DirectoryWatcher();
            directoryWatcher.Created += OnCreated;
            directoryWatcher.Renamed += OnRenamed;
            directoryWatcher.Watch(SourceFolder, FileFilter);
        }

        static void OnCreated(object sender, FileWatchEventArgs args)
        {
            EnqueueFile(args.FileInfo);
        }

        static void OnRenamed(object sender, FileWatchEventArgs args)
        {
            EnqueueFile(args.FileInfo);
        }

        static void EnqueueFile(FileInfo fileInfo)
        {
            byte[] fileAllBytes = ReadFile(fileInfo);
            Guid fileId = Guid.NewGuid();
            double parts = (double)fileAllBytes.Length / ChunkSize;
            int totalParts = (int)Math.Ceiling(parts);
            for (int i = 0; i < fileAllBytes.Length; i += ChunkSize)
            {
                int length = Math.Min(fileAllBytes.Length - i, ChunkSize);
                byte[] buffer = new byte[length];
                Array.Copy(fileAllBytes, i, buffer, 0, length);
                var messageBody = new FileMessageBody()
                {
                    Id = fileId,
                    Content = buffer,
                    FileName = fileInfo.Name,
                    Part = i / ChunkSize + 1,
                    TotalParts = totalParts,
                    TotalBytes = fileAllBytes.Length,
                };

                messagesQueue.Enqueue(messageBody);
            }

            messageAdded.Set();
        }

        static byte[] ReadFile(FileInfo fileInfo)
        {
            byte[] buffer = null;
            byte maxTriesCount = 10;
            byte triesCount = 0;
            bool read = false;
            while (triesCount < maxTriesCount && !read)
            {
                try
                {
                    buffer = File.ReadAllBytes(fileInfo.FullName);
                    read = true;
                }
                catch (Exception)
                {
                    triesCount++;
                    Thread.Sleep(100);
                }
            }

            if (buffer == null)
            {
                throw new IOException("Cannot read file");
            }

            return buffer;
        }
    }
}
