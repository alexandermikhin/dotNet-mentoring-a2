using FeedManager.Task1.FeedValidators;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;

namespace FeedManager.Task3.Factories
{
    public interface IFeedFactory<T> where T : TradeFeed
    {
        IFeedMatcher<T> CreateFeedMatcher();
        IFeedValidator<T> CreateFeedValidator();
    }
}
