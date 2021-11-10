using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using InputServer;

namespace Client
{
    class Program
    {
        const string QueueName = @".\Private$\MessageQueue";
        const string DestinationFolder = @"D:\dotNetlab\Temp\pdfs_processed\";

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
                    File.WriteAllBytes(DestinationFolder + fileMessageBody.FileName, fileMessageBody.Content);
                    Console.WriteLine("File was processed " + fileMessageBody.FileName);
                }
            }
        }
    }
}
