using System;

namespace StockExchange.Task1
{
    public class RedSocks: IPlayer
    {
        Players players;

        public string Name => "RedSocks";

        public RedSocks(Players players)
        {
            this.players = players;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return players.SellOffer(this, stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return players.BuyOffer(this, stockName, numberOfShares);
        }
    }
}
