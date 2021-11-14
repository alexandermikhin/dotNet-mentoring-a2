using System;
using System.IO;

namespace DirectoryWatch
{
    public class DirectoryWatcher
    {
        public event EventHandler<FileWatchEventArgs> Created;
        public event EventHandler<FileWatchEventArgs> Renamed;

        public void Watch(string path, string filter)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;

            watcher.NotifyFilter = NotifyFilters.LastWrite
                | NotifyFilters.FileName;

            watcher.Filter = filter;

            watcher.Created += OnCreated;
            watcher.Renamed += OnRenamed;

            watcher.EnableRaisingEvents = true;
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            var args = new FileWatchEventArgs(new FileInfo(e.FullPath));
            this.Created?.Invoke(this, args);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            var args = new FileWatchEventArgs(new FileInfo(e.FullPath));
            this.Renamed?.Invoke(this, args);
        }
    }
}
