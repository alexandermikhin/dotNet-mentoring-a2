namespace Server
{
    class ServerConfig
    {
        public string QueueName { get; set; }

        public string SourceFolder { get; set; }

        public string FileFilter { get; set; }

        public int ChunkSize { get; set; }
    }
}
