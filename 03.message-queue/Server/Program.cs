using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server");
            var config = new ServerConfig()
            {
                ChunkSize = 10000,
                FileFilter = "*.pdf",
                QueueName = Constants.QueueName,
                SourceFolder = @"D:\Temp\pdf\"
            };

            var server = new Server(config);
            server.Start();
        }
    }
}
