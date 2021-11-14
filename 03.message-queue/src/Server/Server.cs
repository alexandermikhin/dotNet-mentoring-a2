using System;
using System.Collections.Concurrent;
using System.IO;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;
using DirectoryWatch;

namespace Server
{
    internal class Server
    {
        readonly DirectoryWatcher directoryWatcher;
        readonly ServerConfig config;
        readonly ConcurrentQueue<FileMessageBody> messagesQueue = new ConcurrentQueue<FileMessageBody>();
        readonly AutoResetEvent messageAdded = new AutoResetEvent(false);

        public Server(DirectoryWatcher directoryWatcher, ServerConfig config)
        {
            this.directoryWatcher = directoryWatcher;
            this.config = config;
        }

        public void Start()
        {
            CreateMessageQueue();
            var watchTask = Task.Factory.StartNew(WatchDirectory);
            var serverTask = Task.Factory.StartNew(Listen);
            Task.WaitAll(watchTask, serverTask);
        }

        void CreateMessageQueue()
        {
            Console.WriteLine("Creating messsage queue");
            if (!MessageQueue.Exists(config.QueueName))
            {
                MessageQueue.Create(config.QueueName);
            }
        }

        void Listen()
        {
            while (true)
            {
                messageAdded.WaitOne();
                using (var serverQueue = new MessageQueue(config.QueueName))
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

        void WatchDirectory()
        {
            Console.WriteLine("Watching directory...");
            directoryWatcher.Created += OnCreated;
            directoryWatcher.Renamed += OnRenamed;
            try
            {
                directoryWatcher.Watch(config.SourceFolder, config.FileFilter);
            }
            catch
            {
                Console.WriteLine("Error during watching directory " + config.SourceFolder);
                directoryWatcher.Created -= OnCreated;
                directoryWatcher.Renamed -= OnRenamed;
            }
        }

        void OnCreated(object sender, FileWatchEventArgs args)
        {
            EnqueueFile(args.FileInfo);
        }

        void OnRenamed(object sender, FileWatchEventArgs args)
        {
            EnqueueFile(args.FileInfo);
        }

        void EnqueueFile(FileInfo fileInfo)
        {
            byte[] fileAllBytes = ReadFile(fileInfo);
            Guid fileId = Guid.NewGuid();
            double parts = (double)fileAllBytes.Length / config.ChunkSize;
            int totalParts = (int)Math.Ceiling(parts);
            for (int i = 0; i < fileAllBytes.Length; i += config.ChunkSize)
            {
                int length = Math.Min(fileAllBytes.Length - i, config.ChunkSize);
                byte[] buffer = new byte[length];
                Array.Copy(fileAllBytes, i, buffer, 0, length);
                var messageBody = new FileMessageBody()
                {
                    Id = fileId,
                    Content = buffer,
                    FileName = fileInfo.Name,
                    Part = i / config.ChunkSize + 1,
                    TotalParts = totalParts,
                    TotalBytes = fileAllBytes.Length,
                };

                messagesQueue.Enqueue(messageBody);
            }

            messageAdded.Set();
        }

        byte[] ReadFile(FileInfo fileInfo)
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
