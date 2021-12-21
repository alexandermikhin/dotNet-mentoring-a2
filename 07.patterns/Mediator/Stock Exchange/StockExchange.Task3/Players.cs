using System.Collections.Generic;

namespace StockExchange.Task3
{
    public class Players
    {
        public RedSocks RedSocks { get; set; }
        public Blossomers Blossomers { get; set; }

        readonly List<Offer> sellOffers = new List<Offer>();
        readonly List<Offer> buyOffers = new List<Offer>();
        readonly List<Player> subscribers = new List<Player>();

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
            var offer = buyOffers.Find(r => r.From != player.Name && r.Name == stockName && r.SharesNumber == numberOfShares);
            if (offer != null)
            {
                buyOffers.Remove(offer);
                var deal = new Deal()
                {
                    BuyerName = offer.From,
                    SellerName = player.Name,
                    SharesNumber = offer.SharesNumber,
                };

                Notify(deal);
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
                var deal = new Deal()
                {
                    BuyerName = player.Name,
                    SellerName = offer.From,
                    SharesNumber = offer.SharesNumber,
                };

                Notify(deal);
                result = true;
            }
            else
            {
                buyOffers.Add(new Offer() { From = player.Name, Name = stockName, SharesNumber = numberOfShares });
            }

            return result;
        }

        internal void Subsribe(Player player)
        {
            subscribers.Add(player);
        }

        private void Notify(Deal deal)
        {
            foreach (var subscriber in subscribers)
            {
                if (subscriber.Name == deal.BuyerName)
                {
                    subscriber.ChangeBoughShares(deal.SharesNumber);
                }

                if (subscriber.Name == deal.SellerName)
                {
                    subscriber.ChangeSoldShares(deal.SharesNumber);
                }
            }
        }

        class Deal
        {
            public string BuyerName { get; set; }
            public string SellerName { get; set; }
            public int SharesNumber { get; set; }
        }

        class Offer
        {
            public string From { get; set; }
            public string Name { get; set; }
            public int SharesNumber { get; set; }
        }
    }
}
