using FeedManager.Task1.Feeds;
using FeedManager.Task1.FeedValidators;

namespace FeedManager.Task1.FeedImporters
{
    public class EmFeedValidator : BaseFeedValidator, IFeedValidator<EmFeed>
    {
        readonly decimal minSedolValue = 0;
        readonly decimal maxSedolValue = 100;
        readonly decimal minAssetValue = 0;

        public ValidateResult Validate(EmFeed feed)
        {
            validateResult = new ValidateResult();
            ValidateIds(feed);
            ValidatePrice(feed);
            ValidateSedol(feed);
            ValidateAssetValue(feed);

            UpdateValidState();

            return validateResult;
        }

        private void ValidateSedol(EmFeed feed)
        {
            if (feed.Sedol <= minSedolValue || feed.Sedol >= maxSedolValue)
            {
                validateResult.Errors.Add(ErrorCode.PropertyRangeError(nameof(feed.Sedol), minSedolValue, maxSedolValue));
            }
        }

        private void ValidateAssetValue(EmFeed feed)
        {
            if (feed.AssetValue <= minAssetValue || feed.AssetValue >= feed.Sedol)
            {
                validateResult.Errors.Add(ErrorCode.PropertyRangeError(nameof(feed.AssetValue), minAssetValue, feed.Sedol));
            }
        }
    }
}
