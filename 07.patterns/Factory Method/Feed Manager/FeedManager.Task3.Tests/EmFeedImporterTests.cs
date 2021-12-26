using FeedManager.Task2.Feeds;
using FeedManager.Task2.Importers;
using FeedManager.Task3.Tests.Database;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FeedManager.Task3.Tests
{
    [TestClass]
    public class EmFeedImporterTests
    {
        private FakeDatabaseRepository repository;
        private EmFeedImporter feedImporter;

        public EmFeedImporterTests()
        {
            repository = new FakeDatabaseRepository();
            feedImporter = new EmFeedImporter(repository);
        }

        [TestMethod]
        public void Should_save_error_for_invalid_feed()
        {
            var feed = GetInvalidFeed();
            feedImporter.Import(new[] { feed });

            var errors = repository.GetErrors(feed.StagingId);
            errors.Should().NotBeEmpty();
        }

        [TestMethod]
        public void Should_save_valid_feed()
        {
            var feed = CreateValidFeed();
            feedImporter.Import(new[] { feed });

            var savedFeeds = repository.LoadFeeds<EmFeed>();

            savedFeeds.Should().NotBeEmpty();
            savedFeeds.Count.Should().Be(1);
            savedFeeds.First().Should().BeEquivalentTo(feed);
        }

        [TestMethod]
        public void Should_not_save_duplicated_feed()
        {
            var feed = CreateValidFeed();
            feedImporter.Import(new[] { feed });

            var copy = GetDuplicateFeed(feed);
            feedImporter.Import(new[] { copy });

            var savedFeeds = repository.LoadFeeds<EmFeed>();

            savedFeeds.Should().NotBeEmpty();
            savedFeeds.Count.Should().Be(1);
            savedFeeds.First().Should().BeEquivalentTo(feed);
        }

        [TestMethod]
        public void Should_save_not_duplicated_feed()
        {
            var feed = CreateValidFeed();
            feedImporter.Import(new[] { feed });

            var copy = GetDifferentFeed(feed);
            feedImporter.Import(new[] { copy });

            var savedFeeds = repository.LoadFeeds<EmFeed>();

            savedFeeds.Should().NotBeEmpty();
            savedFeeds.Count.Should().Be(2);
            savedFeeds.Should().BeEquivalentTo(new[] { feed, copy });
        }

        [TestCleanup()]
        public void Cleanup()
        {
            repository.Clean();
        }

        private EmFeed GetDuplicateFeed(EmFeed feed)
        {
            return new EmFeed
            {
                SourceAccountId = feed.SourceAccountId,
                StagingId = feed.StagingId,
                CurrentPrice = 45M
            };
        }

        private EmFeed GetDifferentFeed(EmFeed feed)
        {
            return new EmFeed
            {
                SourceAccountId = feed.SourceAccountId + 1,
                StagingId = feed.StagingId + 1,
                CounterpartyId = feed.CounterpartyId,
                PrincipalId = feed.PrincipalId,
                CurrentPrice = feed.CurrentPrice,
                Sedol = feed.Sedol,
                AssetValue = feed.AssetValue,
                SourceTradeRef = feed.SourceTradeRef,
                ValuationDate = feed.ValuationDate
            };
        }

        private EmFeed GetInvalidFeed()
        {
            var feed = CreateValidFeed();
            feed.CounterpartyId = -1;
            return feed;
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
