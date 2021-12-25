using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();                    
        }
    }
}