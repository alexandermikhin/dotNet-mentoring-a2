using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Threading.Tasks;
using Server;

namespace Client
{
    class Program
    {
        const string QueueName = @".\Private$\MessageQueue";
        const string DestinationFolder = @"D:\dotNetlab\Temp\pdfs_processed\";
        static readonly ConcurrentDictionary<Guid, SortedList<int, FileMessageBody>> messagesDictionary = new ConcurrentDictionary<Guid, SortedList<int, FileMessageBody>>();
        static object syncLock = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Client");
            Console.WriteLine("Create message queue");
            CreateMessageQueue();
            Console.WriteLine("Start client");
            var task = Task.Factory.StartNew(Client);
            task.Wait();
        }

        static void CreateMessageQueue()
        {
            if (!MessageQueue.Exists(QueueName))
            {
                MessageQueue.Create(QueueName);
            }
        }

        static void Client()
        {
            using (var queue = new MessageQueue(QueueName))
            {
                queue.Formatter = new BinaryMessageFormatter();
                while (true)
                {
                    var message = queue.Receive();
                    var fileMessageBody = (FileMessageBody)message.Body;
                    var consoleMessage = $"Received file {fileMessageBody.FileName}.";
                    if (fileMessageBody.TotalParts > 1)
                    {
                        consoleMessage += $" Part {fileMessageBody.Part} of {fileMessageBody.TotalParts}.";
                    }

                    Console.WriteLine(consoleMessage);
                    lock (syncLock)
                    {
                        messagesDictionary.AddOrUpdate(fileMessageBody.Id,
                            new SortedList<int, FileMessageBody>(fileMessageBody.TotalParts) { { fileMessageBody.Part, fileMessageBody } },
                            (key, messages) =>
                            {
                                messages.Add(fileMessageBody.Part, fileMessageBody);
                                return messages;
                            });
                    }

                    TryWriteFile(fileMessageBody);
                }
            }
        }

        static void TryWriteFile(FileMessageBody messageBody)
        {
            lock (syncLock)
            {
                if (messagesDictionary.TryGetValue(messageBody.Id, out var messages))
                {
                    if (messages.Count == messageBody.TotalParts)
                    {
                        byte[] allBytes = new byte[messageBody.TotalBytes];
                        int pointer = 0;
                        foreach (var message in messages.Values)
                        {
                            Array.Copy(message.Content, 0, allBytes, pointer, message.Content.Length);
                            pointer += message.Content.Length;
                        }

                        File.WriteAllBytes(DestinationFolder + messageBody.FileName, allBytes);
                        messagesDictionary.TryRemove(messageBody.Id, out var _);
                        Console.WriteLine("File was processed " + messageBody.FileName);
                    }
                }
            }
        }
    }
}
