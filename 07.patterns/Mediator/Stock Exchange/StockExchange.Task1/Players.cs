using System.Collections.Generic;

namespace StockExchange.Task1
{
    public class Players
    {
        public RedSocks RedSocks { get; set; }
        public Blossomers Blossomers { get; set; }

        readonly Requests redSocksRequests = new Requests();
        readonly Requests blossomersRequests = new Requests();

        public Players()
        { 
        }

        public Players(RedSocks redSocks, Blossomers blossomers)
        {
            RedSocks = redSocks;
            Blossomers = blossomers;
        }

        internal bool SellOffer(Player player, string stockName, int numberOfShares)
        {
            var result = false;
            var buyRequests = player.Name == "RedSocks" ? blossomersRequests.BuyRequests : redSocksRequests.BuyRequests;
            var offer = buyRequests.Find(r => r.Name == stockName && r.SharesNumber == numberOfShares);
            if (offer != null)
            {
                buyRequests.Remove(offer);
                result = true;
            }
            else
            {
                var sellRequests = player.Name == "RedSocks" ? redSocksRequests.SellRequests : blossomersRequests.SellRequests;
                sellRequests.Add(new Offer() { Name = stockName, SharesNumber = numberOfShares });
            }

            return result;
        }

        internal bool BuyOffer(Player player, string stockName, int numberOfShares)

        {
            var result = false;
            var sellRequests = player.Name == RedSocks.Name ? blossomersRequests.SellRequests : redSocksRequests.SellRequests;
            var offer = sellRequests.Find(r => r.Name == stockName && r.SharesNumber == numberOfShares);
            if (offer != null)
            {
                sellRequests.Remove(offer);
                result = true;
            }
            else
            {
                var buyRequests = player.Name == RedSocks.Name ? redSocksRequests.BuyRequests : blossomersRequests.BuyRequests;
                buyRequests.Add(new Offer() { Name = stockName, SharesNumber = numberOfShares });
            }

            return result;
        }

        class Offer
        {
            public string Name { get; set; }
            public int SharesNumber { get; set; }
        }

        class Requests
        {
            readonly List<Offer> buyRequests = new List<Offer>();
            readonly List<Offer> sellRequests = new List<Offer>();

            public List<Offer> BuyRequests => buyRequests;
            public List<Offer> SellRequests => sellRequests;
        }
    }
}
