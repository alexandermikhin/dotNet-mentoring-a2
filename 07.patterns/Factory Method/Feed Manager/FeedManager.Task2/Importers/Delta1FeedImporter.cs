using FeedManager.Task1.FeedImporters;
using FeedManager.Task1.FeedValidators;
using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;

namespace FeedManager.Task2.Importers
{
    public class Delta1FeedImporter: BaseFeedImporter<Delta1Feed>
    {
        public Delta1FeedImporter(IDatabaseRepository database): base(database)
        {
        }
        protected override IFeedMatcher<Delta1Feed> CreateFeedMatcher()
        {
            return new Delta1FeedMatcher();
        }

        protected override IFeedValidator<Delta1Feed> CreateFeedValidator()
        {
            return new Delta1FeedValidator();
        }
    }
}