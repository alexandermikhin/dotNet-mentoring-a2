using FeedManager.Task2.Database;
using FeedManager.Task2.Feeds;
using FeedManager.Task3.Factories;

namespace FeedManager.Task2.Importers
{
    public class Delta1FeedImporter : BaseFeedImporter<Delta1Feed>
    {
        public Delta1FeedImporter(IDatabaseRepository database) : base(database, new Delta1FeedFactory())
        {
        }
    }
}