using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("User > ");
            var user = Console.ReadLine();
            var client = new Client(user);
            client.Start();
        }
    }
}
