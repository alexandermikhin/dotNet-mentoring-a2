using System;

namespace FeedManager.Task1.Feeds
{
    public class Delta1Feed : TradeFeed
    {
        public string Isin { get; set; }
        
        public DateTime MaturityDate { get; set; }
    }
}
