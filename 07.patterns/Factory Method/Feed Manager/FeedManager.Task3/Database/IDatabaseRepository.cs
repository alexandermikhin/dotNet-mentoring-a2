using FeedManager.Task2.Feeds;
using System;
using System.Collections.Generic;

namespace FeedManager.Task2.Database
{
    public interface IDatabaseRepository
    {
        List<T> LoadFeeds<T>()
            where T : TradeFeed;

        void SaveFeed<T>(T feed)
            where T : TradeFeed;

        void SaveErrors(int feedStagingId, List<String> errors);
    }
}