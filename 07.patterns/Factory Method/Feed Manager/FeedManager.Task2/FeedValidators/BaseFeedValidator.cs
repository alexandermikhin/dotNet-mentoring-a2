using System;
using FeedManager.Task1.FeedImporters;
using FeedManager.Task2.Feeds;

namespace FeedManager.Task1.FeedValidators
{
    public abstract class BaseFeedValidator
    {
        protected ValidateResult validateResult;

        protected void ValidateIds(TradeFeed feed)
        {
            if (feed.StagingId < 1 || feed.CounterpartyId < 1 || feed.PrincipalId < 1 || feed.SourceAccountId < 1)
            {
                validateResult.Errors.Add(ErrorCode.IdIsNotValidMessage);
            }
        }

        protected void ValidatePrice(TradeFeed feed)
        {
            if (feed.CurrentPrice < 0 || !HasValidFractionPart(feed.CurrentPrice, 2))
            {
                validateResult.Errors.Add(ErrorCode.PriceIsNotValid);
            }
        }

        protected void UpdateValidState()
        {
            validateResult.IsValid = validateResult.Errors.Count == 0;
        }

        private bool HasValidFractionPart(decimal number, int decimals)
        {
            return number - Math.Round(number, decimals) == 0;
        }
    }
}
