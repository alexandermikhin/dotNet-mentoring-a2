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
    public class Delta1FeedImporterTests
    {
        private FakeDatabaseRepository repository;
        private Delta1FeedImporter feedImporter;

        public Delta1FeedImporterTests()
        {
            repository = new FakeDatabaseRepository();
            feedImporter = new Delta1FeedImporter(repository);
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

            var savedFeeds = repository.LoadFeeds<Delta1Feed>();

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

            var savedFeeds = repository.LoadFeeds<Delta1Feed>();

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

            var savedFeeds = repository.LoadFeeds<Delta1Feed>();

            savedFeeds.Should().NotBeEmpty();
            savedFeeds.Count.Should().Be(2);
            savedFeeds.Should().BeEquivalentTo(new[] { feed, copy });
        }

        [TestCleanup()]
        public void Cleanup()
        {
            repository.Clean();
        }

        private Delta1Feed GetDuplicateFeed(Delta1Feed feed)
        {
            return new Delta1Feed
            {
                CounterpartyId = feed.CounterpartyId,
                PrincipalId = feed.PrincipalId,
                CurrentPrice = 45M
            };
        }

        private Delta1Feed GetDifferentFeed(Delta1Feed feed)
        {
            return new Delta1Feed
            {
                CounterpartyId = feed.CounterpartyId + 1,
                PrincipalId = feed.PrincipalId + 1,
                CurrentPrice = feed.CurrentPrice + 2,
                SourceAccountId = feed.SourceAccountId,
                StagingId = feed.StagingId,
                Isin = feed.Isin,
                MaturityDate = feed.MaturityDate,
                SourceTradeRef = feed.SourceTradeRef,
                ValuationDate = feed.ValuationDate
            };
        }

        private Delta1Feed GetInvalidFeed()
        {
            var feed = CreateValidFeed();
            feed.CounterpartyId = -1;
            return feed;
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
