namespace StockExchange.Task4
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
            this.players.DealHappened += OnDealHappened;
        }

        ~Player()
        {
            this.players.DealHappened -= OnDealHappened;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return players.SellOffer(this, stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return players.BuyOffer(this, stockName, numberOfShares);
        }

        private void OnDealHappened(object sender, DealEventArgs eventArgs)
        {
            if (eventArgs.BuyerName == Name)
            {
                boughtShares += eventArgs.SharesNumber;
            }

            if (eventArgs.SellerName == Name)
            {
                soldShares += eventArgs.SharesNumber;
            }
        }
    }
}
