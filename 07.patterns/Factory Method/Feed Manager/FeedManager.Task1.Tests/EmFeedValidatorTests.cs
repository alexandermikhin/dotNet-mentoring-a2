using FeedManager.Task1.FeedImporters;
using FeedManager.Task1.Feeds;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FeedManager.Tests.Task1.Tests
{
    [TestClass]
    public class EmFeedValidatorTests
    {
        private EmFeedValidator feedValidator;

        public EmFeedValidatorTests()
        {
            feedValidator = new EmFeedValidator();
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
        public void Should_validate_invalid_ids(Action<EmFeed> action)
        {
            var feed = CreateValidFeed();
            action(feed);
            var result = feedValidator.Validate(feed);
            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<Action<EmFeed>[]> ValidateInvalidPropertiesData
        {
            get
            {
                yield return new Action<EmFeed>[] { feed => feed.StagingId = -10 };
                yield return new Action<EmFeed>[] { feed => feed.CounterpartyId = -10 };
                yield return new Action<EmFeed>[] { feed => feed.PrincipalId = -10 };
                yield return new Action<EmFeed>[] { feed => feed.SourceAccountId = -10 };
                yield return new Action<EmFeed>[] { feed => feed.CurrentPrice = -10 };
                yield return new Action<EmFeed>[] { feed => feed.CurrentPrice = 12.3444M };
                yield return new Action<EmFeed>[] { feed => feed.Sedol = -3M };
                yield return new Action<EmFeed>[] { feed => feed.Sedol = 130M };
                yield return new Action<EmFeed>[] { feed => feed.AssetValue = -13M };
                yield return new Action<EmFeed>[] { feed =>
                {
                    feed.AssetValue = 200M;
                    feed.Sedol = 100M;
                } };
            }
        }

        private EmFeed CreateValidFeed()
        {
            return new EmFeed
            {
                CounterpartyId = 10,
                SourceAccountId = 11,
                StagingId = 12,
                PrincipalId = 13,
                CurrentPrice = 12.34M,
                Sedol = 50M,
                AssetValue = 10M,
                SourceTradeRef = "ref",
                ValuationDate = DateTime.Now
            };
        }
    }
}
