using System.Collections.Generic;

namespace StockExchange.Task2
{
    public class Players
    {
        public RedSocks RedSocks { get; set; }
        public Blossomers Blossomers { get; set; }
        public RossStones RossStones { get; set; }

        readonly List<Offer> sellOffers = new List<Offer>();
        readonly List<Offer> buyOffers = new List<Offer>();

        public Players()
        {
        }

        public Players(RedSocks redSocks, Blossomers blossomers, RossStones rossStones)
        {
            RedSocks = redSocks;
            Blossomers = blossomers;
            RossStones = rossStones;
        }

        internal bool SellOffer(Player player, string stockName, int numberOfShares)
        {
            var result = false;
            var offer = buyOffers.Find(r => r.From != player.Name && r.Name == stockName && r.SharesNumber == numberOfShares);
            if (offer != null)
            {
                buyOffers.Remove(offer);
                result = true;
            }
            else
            {
                sellOffers.Add(new Offer() { From = player.Name, Name = stockName, SharesNumber = numberOfShares });
            }

            return result;
        }

        internal bool BuyOffer(Player player, string stockName, int numberOfShares)

        {
            var result = false;
            var offer = sellOffers.Find(r => r.From != player.Name && r.Name == stockName && r.SharesNumber == numberOfShares);
            if (offer != null)
            {
                sellOffers.Remove(offer);
                result = true;
            }
            else
            {
                buyOffers.Add(new Offer() { From = player.Name, Name = stockName, SharesNumber = numberOfShares });
            }

            return result;
        }

        class Offer
        {
            public string From { get; set; }
            public string Name { get; set; }
            public int SharesNumber { get; set; }
        }
    }
}
