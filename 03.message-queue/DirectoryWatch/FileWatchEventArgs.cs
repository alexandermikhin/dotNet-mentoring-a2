using System;
using System.IO;

namespace DirectoryWatch
{
    public class FileWatchEventArgs : EventArgs
    {
        readonly FileInfo fileInfo;

        public FileWatchEventArgs(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public FileInfo FileInfo => fileInfo;
    }
}
