using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }
    }
}
