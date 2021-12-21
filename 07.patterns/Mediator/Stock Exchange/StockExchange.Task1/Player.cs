using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task1
{
    interface IPlayer
    {
        public string Name { get; }
        public bool SellOffer(string stockName, int numberOfShares);
        public bool BuyOffer(string stockName, int numberOfShares);
    }
}
