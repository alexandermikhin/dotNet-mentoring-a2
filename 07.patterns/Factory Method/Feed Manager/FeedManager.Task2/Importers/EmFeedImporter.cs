using System.Collections.Generic;
using FeedManager.Task1.FeedImporters;
using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;

namespace FeedManager.Task2.Importers
{
    public class EmFeedImporter
    {
        private readonly IDatabaseRepository database;

        public EmFeedImporter(IDatabaseRepository database)
        {
            this.database = database;
        }

        public void Import(IEnumerable<EmFeed> feeds)
        {
            var matcher = new EmFeedMatcher();
            var validator = new EmFeedValidator();
            var existingFeeds = database.LoadFeeds<EmFeed>();
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
