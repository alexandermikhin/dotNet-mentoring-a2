using FeedManager.Task1.FeedImporters;
using FeedManager.Task2.Feeds;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AbstractFactory.Tests
{
    [TestClass]
    public class Delta1FeedValidatorTests
    {
        private Delta1FeedValidator feedValidator;

        public Delta1FeedValidatorTests()
        {
            feedValidator = new Delta1FeedValidator();
        }

        [TestMethod]
        public void Should_not_validate_valid_delta_1_feed()
        {
            var feed = CreateValidFeed();
            var result = feedValidator.Validate(feed);
            result.IsValid.Should().BeTrue();
        }

        [DataTestMethod]
        [DynamicData(nameof(ValidateInvalidPropertiesData), DynamicDataSourceType.Property)]
        public void Should_validate_invalid_ids(Action<Delta1Feed> action)
        {
            var feed = CreateValidFeed();
            action(feed);
            var result = feedValidator.Validate(feed);
            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<Action<Delta1Feed>[]> ValidateInvalidPropertiesData
        {
            get
            {
                yield return new Action<Delta1Feed>[] { feed => feed.StagingId = -10 };
                yield return new Action<Delta1Feed>[] { feed => feed.CounterpartyId = -10 };
                yield return new Action<Delta1Feed>[] { feed => feed.PrincipalId = -10 };
                yield return new Action<Delta1Feed>[] { feed => feed.SourceAccountId = -10 };
                yield return new Action<Delta1Feed>[] { feed => feed.CurrentPrice = -10 };
                yield return new Action<Delta1Feed>[] { feed => feed.CurrentPrice = 12.3444M };
                yield return new Action<Delta1Feed>[] { feed => feed.Isin = "U9311421039" };
                yield return new Action<Delta1Feed>[] { feed => feed.Isin = "US9311421039" };
                yield return new Action<Delta1Feed>[] { feed =>
                {
                    feed.MaturityDate = DateTime.Now.AddDays(-1);
                    feed.ValuationDate = DateTime.Now;
                } };
            }
        }

        private Delta1Feed CreateValidFeed()
        {
            return new Delta1Feed
            {
                CounterpartyId = 10,
                SourceAccountId = 11,
                StagingId = 12,
                PrincipalId = 13,
                CurrentPrice = 12.34M,
                Isin = "US9311421039",
                MaturityDate = DateTime.Now.AddDays(1),
                SourceTradeRef = "ref",
                ValuationDate = DateTime.Now
            };
        }
    }
}
