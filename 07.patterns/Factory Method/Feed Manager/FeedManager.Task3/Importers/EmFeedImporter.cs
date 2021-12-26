using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using FeedManager.Task3.Factories;

namespace FeedManager.Task2.Importers
{
    public class EmFeedImporter : BaseFeedImporter<EmFeed>
    {
        public EmFeedImporter(IDatabaseRepository database) : base(database, new EmFeedFactory())
        {
        }
    }
}
