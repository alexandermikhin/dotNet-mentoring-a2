using System;
using DirectoryWatch;

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

            var directoryWatcher = new DirectoryWatcher();
            var server = new Server(directoryWatcher, config);
            server.Start();
        }
    }
}
