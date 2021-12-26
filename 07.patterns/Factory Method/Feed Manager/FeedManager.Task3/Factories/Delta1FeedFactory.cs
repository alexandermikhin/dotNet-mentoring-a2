using System;
using System.Collections.Generic;
using System.Text;
using FeedManager.Task1.FeedImporters;
using FeedManager.Task1.FeedValidators;
using FeedManager.Task2.Feeds;
using FeedManager.Task2.Matchers;

namespace FeedManager.Task3.Factories
{
    public class Delta1FeedFactory : IFeedFactory<Delta1Feed>
    {
        public IFeedMatcher<Delta1Feed> CreateFeedMatcher()
        {
            return new Delta1FeedMatcher();
        }

        public IFeedValidator<Delta1Feed> CreateFeedValidator()
        {
            return new Delta1FeedValidator();
        }
    }
}
