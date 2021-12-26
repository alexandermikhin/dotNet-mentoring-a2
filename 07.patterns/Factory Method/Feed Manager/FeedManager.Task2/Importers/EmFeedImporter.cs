using FeedManager.Task1.FeedImporters;
using FeedManager.Task1.FeedValidators;
using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;

namespace FeedManager.Task2.Importers
{
    public class EmFeedImporter: BaseFeedImporter<EmFeed>
    {
        public EmFeedImporter(IDatabaseRepository database): base(database)
        {
        }

        protected override IFeedMatcher<EmFeed> CreateFeedMatcher()
        {
            return new EmFeedMatcher();
        }

        protected override IFeedValidator<EmFeed> CreateFeedValidator()
        {
            return new EmFeedValidator();
        }
    }
}
