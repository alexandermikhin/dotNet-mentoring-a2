using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chat.Shared;
using System.Threading.Tasks;

namespace Chat.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("User > ");
            //var user = Console.ReadLine();
            var messagesRepository = new MessagesRepository("client");
            //var client = new ClientSocket(messagesRepository);
            var client = new Client(messagesRepository);
            client.Start();
        }
    }
}
