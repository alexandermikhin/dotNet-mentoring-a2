using System.Collections.Generic;

namespace FeedManager.Task1.FeedImporters
{
    public class ValidateResult
    {
        public bool IsValid { get; set; }

        public List<string> Errors { get; set; }
    }
}
