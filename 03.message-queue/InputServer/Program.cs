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
        static readonly ConcurrentQueue<string> filesQueue = new ConcurrentQueue<string>();
        static readonly AutoResetEvent fileAdded = new AutoResetEvent(false);

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
                fileAdded.WaitOne();
                using (var serverQueue = new MessageQueue(QueueName))
                {
                    serverQueue.Formatter = new BinaryMessageFormatter();

                    while (!filesQueue.IsEmpty)
                    {
                        if (filesQueue.TryDequeue(out var fileName))
                        {
                            byte[] bf = File.ReadAllBytes(SourceFolder + fileName);
                            var messageBody = new FileMessageBody()
                            {
                                Id = Guid.NewGuid(),
                                Content = bf,
                                FileName = fileName,
                            };
                            serverQueue.Send(messageBody);
                            Console.WriteLine($"File {fileName} was put in queue.");
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
            filesQueue.Enqueue(fileInfo.Name);
            fileAdded.Set();
        }
    }
}
