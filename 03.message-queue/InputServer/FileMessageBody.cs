using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputServer
{
    [Serializable]
    public class FileMessageBody
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public byte[] Content { get; set; }

        public int Part { get; set; }

        public int TotalParts { get; set; }

        public int TotalBytes { get; set; }
    }
}
