using FeedManager.Task2.Feeds;

namespace FeedManager.Task2.Matchers
{
    public class Delta1FeedMatcher : IFeedMatcher<Delta1Feed>
    {
        public bool Match(Delta1Feed current, Delta1Feed other)
        {
            return (current.CounterpartyId + current.PrincipalId) ==
                (other.CounterpartyId + other.PrincipalId);
        }
    }
}
