using FeedManager.Task2.Feeds;

namespace FeedManager.Task2.Matchers
{
    public class EmFeedMatcher : IFeedMatcher<EmFeed>
    {
        public bool Match(EmFeed current, EmFeed other)
        {
            return (current.SourceAccountId == other.SourceAccountId) || (current.StagingId == other.StagingId);
        }
    }
}
