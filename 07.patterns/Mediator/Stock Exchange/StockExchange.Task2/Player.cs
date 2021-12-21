namespace StockExchange.Task2
{
    public abstract class Player
    {
        readonly Players players;

        public string Name { get; }

        internal Player(string name, Players players)
        {
            Name = name;
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
