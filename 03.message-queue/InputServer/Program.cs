using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Threading;

namespace InputServer
{
    class Program
    {
        const string QueueName = @".\Private$\MessageQueue";

        static void Main(string[] args)
        {
            CreateMessageQueue();
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
            int count = 0;
            using (var serverQueue = new MessageQueue(QueueName))
            {
                while (true)
                {
                    count++;
                    serverQueue.Send("Server message " + count);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
