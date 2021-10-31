using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Chat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = null;
            try
            {
                server = new Server();
                server.Start();
            }
            catch (Exception ex)
            {
                if (server != null)
                {
                    server.Disconnect();
                }

                Console.WriteLine("Exception on server " + ex.Message);
            }
        }
    }
}
