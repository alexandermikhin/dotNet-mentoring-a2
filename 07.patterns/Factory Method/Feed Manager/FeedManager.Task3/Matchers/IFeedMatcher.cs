using FeedManager.Task2.Feeds;

namespace FeedManager.Task2.Matchers
{
    public interface IFeedMatcher<T>
        where T : TradeFeed
    {
        bool Match(T current, T other);
    }
}
