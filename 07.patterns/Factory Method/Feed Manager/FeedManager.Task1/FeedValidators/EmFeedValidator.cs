using FeedManager.Task1.Feeds;
using FeedManager.Task1.FeedValidators;

namespace FeedManager.Task1.FeedImporters
{
    public class EmFeedValidator : IFeedValidator<EmFeed>
    {
        public ValidateResult Validate(EmFeed feed)
        {
            throw new System.NotImplementedException();
        }
    }
}
