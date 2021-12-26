using System.Text.RegularExpressions;
using FeedManager.Task2.Feeds;
using FeedManager.Task1.FeedValidators;

namespace FeedManager.Task1.FeedImporters
{
    public class Delta1FeedValidator : BaseFeedValidator, IFeedValidator<Delta1Feed>
    {
        readonly Regex isinRegex = new Regex("[A-Z]{2,}\\d{10,}$");

        public ValidateResult Validate(Delta1Feed feed)
        {
            validateResult = new ValidateResult();
            ValidateIds(feed);
            ValidatePrice(feed);
            ValidateIsin(feed);
            ValidateDates(feed);
            UpdateValidState();

            return validateResult;
        }

        private void ValidateIsin(Delta1Feed feed)
        {
            if (feed.Isin != null && !isinRegex.IsMatch(feed.Isin))
            {
                validateResult.Errors.Add(ErrorCode.NotValidIsin);
            }
        }

        private void ValidateDates(Delta1Feed feed)
        {
            if (feed.MaturityDate <= feed.ValuationDate)
            {
                validateResult.Errors.Add("MaturityDate should be bigger than ValuationDate");
            }
        }
    }
}
