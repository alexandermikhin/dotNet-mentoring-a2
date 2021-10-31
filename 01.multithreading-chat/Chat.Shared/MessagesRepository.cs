using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared
{
    public class MessagesRepository
    {
        readonly Random random = new Random();
        readonly List<string> messages = new List<string>();
        readonly string name;

        public MessagesRepository(string type)
        {
            name = type;
            InitMessages();
        }

        public string GetMessage()
        {
            var index = random.Next(0, messages.Count);

            return messages[index];
        }

        private void InitMessages()
        {
            for (var i = 0; i < 100; i++)
            {
                messages.Add($"Message {i} from {name}.");
            }
        }
    }
}
