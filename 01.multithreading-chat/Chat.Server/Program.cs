using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Chat.Shared;

namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var messagesRepository = new MessagesRepository("server");
            //var server = new ServerSocket();
            var server = new Server(messagesRepository);
            server.Start();
        }
    }
}
