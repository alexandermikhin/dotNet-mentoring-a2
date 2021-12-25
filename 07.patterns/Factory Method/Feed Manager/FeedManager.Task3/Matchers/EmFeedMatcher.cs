using FeedManager.Task2.Feeds;
using System;

namespace FeedManager.Task2.Matchers
{
    public class EmFeedMatcher : IFeedMatcher<EmFeed>
    {
        public bool Match(EmFeed current, EmFeed other)
        {
            throw new NotImplementedException();
        }
    }
}
