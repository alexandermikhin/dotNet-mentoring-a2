using FeedManager.Task1.FeedImporters;
using FeedManager.Task1.FeedValidators;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;

namespace FeedManager.Task3.Factories
{
    public class EmFeedFactory : IFeedFactory<EmFeed>
    {
        public IFeedMatcher<EmFeed> CreateFeedMatcher()
        {
            return new EmFeedMatcher();
        }

        public IFeedValidator<EmFeed> CreateFeedValidator()
        {
            return new EmFeedValidator();
        }
    }
}
