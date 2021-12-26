using System.Collections.Generic;
using FeedManager.Task1.FeedValidators;
using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;
using FeedManager.Task3.Factories;

namespace FeedManager.Task2.Importers
{
    public abstract class BaseFeedImporter<T> where T : TradeFeed
    {
        protected readonly IDatabaseRepository database;
        protected readonly IFeedFactory<T> factory;

        protected internal BaseFeedImporter(IDatabaseRepository database, IFeedFactory<T> factory)
        {
            this.database = database;
            this.factory = factory;
        }

        public void Import(IEnumerable<T> feeds)
        {
            var matcher = factory.CreateFeedMatcher();
            var validator = factory.CreateFeedValidator();
            var existingFeeds = database.LoadFeeds<T>();
            foreach (var feed in feeds)
            {
                if (!existingFeeds.Exists(f => matcher.Match(feed, f)))
                {
                    var validateResult = validator.Validate(feed);
                    if (validateResult.IsValid)
                    {
                        database.SaveFeed(feed);
                    }
                    else
                    {
                        database.SaveErrors(feed.StagingId, validateResult.Errors);
                    }
                }
            }
        }
    }
}
