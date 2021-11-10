using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Messaging;
using System.Threading;

namespace InputServer
{
    class Program
    {
        const string QueueName = @".\Private$\MessageQueue";
        const string SourceFolder = @"D:\dotNetlab\Temp\pdfs\";

        static void Main(string[] args)
        {
            Console.WriteLine("Creating messsage queue");
            CreateMessageQueue();
            Console.WriteLine("Starting server...");
            var task = Task.Factory.StartNew(Server);
            task.Wait();
        }

        static void CreateMessageQueue()
        {
            if (!MessageQueue.Exists(QueueName))
            {
                MessageQueue.Create(QueueName);
            }
        }

        static void Server()
        {
            using (var serverQueue = new MessageQueue(QueueName))
            {
                while (true)
                {
                    serverQueue.Formatter = new BinaryMessageFormatter();
                    var fileName = "Learn instruction_mentee.pdf";
                    byte[] bf = File.ReadAllBytes(SourceFolder + fileName);
                    var messageBody = new FileMessageBody()
                    {
                        Id = Guid.NewGuid(),
                        Content = bf,
                        FileName = fileName,
                    };
                    serverQueue.Send(messageBody);
                    Console.WriteLine($"File ${messageBody.FileName} was put in queue.");
                }
            }
        }
    }
}
