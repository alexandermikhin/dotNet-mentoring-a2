using FeedManager.Task1.FeedImporters;
using FeedManager.Task2.Feeds;

namespace FeedManager.Task1.FeedValidators
{
    public interface IFeedValidator<T>
        where T: TradeFeed
    {
        ValidateResult Validate(T feed);
    }
}
