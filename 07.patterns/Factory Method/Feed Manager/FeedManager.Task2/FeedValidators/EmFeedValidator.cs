using FeedManager.Task1.FeedValidators;
using FeedManager.Task2.Feeds;

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
