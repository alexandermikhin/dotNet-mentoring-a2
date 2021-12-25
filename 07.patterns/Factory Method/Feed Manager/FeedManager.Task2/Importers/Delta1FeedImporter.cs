using System.Collections.Generic;
using FeedManager.Task1.FeedImporters;
using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;

namespace FeedManager.Task2.Importers
{
    public class Delta1FeedImporter
    {
        private readonly IDatabaseRepository database;

        public Delta1FeedImporter(IDatabaseRepository database)
        {
            this.database = database;
        }

        public void Import(IEnumerable<Delta1Feed> feeds)
        {
            var matcher = new Delta1FeedMatcher();
            var validator = new Delta1FeedValidator();
            var existingFeeds = database.LoadFeeds<Delta1Feed>();
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