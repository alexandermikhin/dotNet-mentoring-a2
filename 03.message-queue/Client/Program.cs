using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        const string QueueName = @".\Private$\MessageQueue";

        static void Main(string[] args)
        {
            CreateMessageQueue();
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
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                while (true)
                {
                    var message = queue.Receive();
                    Console.WriteLine(message.Body);
                }
            }
        }
    }
}
