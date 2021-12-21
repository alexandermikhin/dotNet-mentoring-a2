namespace StockExchange.Task3
{
    public abstract class Player
    {
        readonly Players players;
        int soldShares;
        int boughtShares;

        public string Name { get; }

        public int SoldShares => soldShares;

        public int BoughtShares => boughtShares;

        internal Player(string name, Players players)
        {
            Name = name;
            this.players = players;
            this.players.Subsribe(this);
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return players.SellOffer(this, stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return players.BuyOffer(this, stockName, numberOfShares);
        }

        public void ChangeBoughShares(int diff)
        {
            boughtShares += diff;
        }

        public void ChangeSoldShares(int diff)
        {
            soldShares += diff;
        }
    }
}
