using System;
using System.Threading.Tasks;
using Server;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client");
            var config = new ClientConfig()
            {
                DestinationFolder = @".\processed",
                QueueName = Constants.QueueName,
            };

            var client = new Client(config);
            var task = Task.Factory.StartNew(client.Serve);
            task.Wait();
        }
    }
}
