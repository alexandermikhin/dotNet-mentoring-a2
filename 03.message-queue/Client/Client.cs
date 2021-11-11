using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using Server;

namespace Client
{
    internal class Client
    {
        readonly ClientConfig config;
        readonly ConcurrentDictionary<Guid, SortedList<int, FileMessageBody>> messagesDictionary = new ConcurrentDictionary<Guid, SortedList<int, FileMessageBody>>();
        object syncLock = new object();

        public Client(ClientConfig config)
        {
            this.config = config;
        }

        public void Serve()
        {
            CreateMessageQueue();
            Console.WriteLine("Start client");
            ReadQueue();
        }

        void CreateMessageQueue()
        {
            Console.WriteLine("Create message queue");
            if (!MessageQueue.Exists(config.QueueName))
            {
                MessageQueue.Create(config.QueueName);
            }
        }

        void ReadQueue()
        {
            using (var queue = new MessageQueue(config.QueueName))
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

        void TryWriteFile(FileMessageBody messageBody)
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

                        WriteFileContents(messageBody.FileName, allBytes);
                        messagesDictionary.TryRemove(messageBody.Id, out var _);
                        Console.WriteLine("File was processed " + messageBody.FileName);
                    }
                }
            }
        }

        void WriteFileContents(string fileName, byte[] content)
        {
            try
            {
                if (!Directory.Exists(config.DestinationFolder))
                {
                    Directory.CreateDirectory(config.DestinationFolder);
                }

                File.WriteAllBytes(Path.Combine(config.DestinationFolder, fileName), content);
            }
            catch (IOException)
            {
                Console.WriteLine("Error during saving file.");
            }
        }
    }
}
