using System;
using System.IO;

namespace DirectoryWatch
{
    public class DirectoryWatcher
    {
        public event EventHandler<FileWatchEventArgs> Changed;
        public event EventHandler<FileWatchEventArgs> Renamed;

        public void Watch(string path, string filter)
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = path;

                watcher.NotifyFilter = NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName;

                watcher.Filter = filter;

                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                watcher.EnableRaisingEvents = true;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var args = new FileWatchEventArgs(new FileInfo(e.FullPath));
            this.Changed?.Invoke(this, args);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            var args = new FileWatchEventArgs(new FileInfo(e.FullPath));
            this.Renamed?.Invoke(this, args);
        }
    }
}
