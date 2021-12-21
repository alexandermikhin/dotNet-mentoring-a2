using System;
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

        internal bool SellOffer(IPlayer player, string stockName, int numberOfShares)
        {
            switch (player.Name)
            {
                case "RedSocks":
                    {
                        var offer = blossomersRequests.BuyRequests.Find(r => r.Name == stockName && r.SharesNumber == numberOfShares);
                        if (offer != null)
                        {
                            blossomersRequests.BuyRequests.Remove(offer);
                            return true;
                        }
                        else
                        {
                            redSocksRequests.SellRequests.Add(new Offer() { Name = stockName, SharesNumber = numberOfShares });
                            return false;
                        }
                    }
                case "Blossomers":
                    {
                        var offer = redSocksRequests.BuyRequests.Find(r => r.Name == stockName && r.SharesNumber == numberOfShares);
                        if (offer != null)
                        {
                            redSocksRequests.BuyRequests.Remove(offer);
                            return true;
                        }
                        else
                        {
                            blossomersRequests.SellRequests.Add(new Offer() { Name = stockName, SharesNumber = numberOfShares });
                            return false;
                        }
                    }
                default:
                    throw new ArgumentException("Provided seller " + player.Name + " is not defined.");
            }
        }

        internal bool BuyOffer(IPlayer player, string stockName, int numberOfShares)
        {
            switch (player.Name)
            {
                case "RedSocks":
                    {
                        var offer = blossomersRequests.SellRequests.Find(r => r.Name == stockName && r.SharesNumber == numberOfShares);
                        if (offer != null)
                        {
                            blossomersRequests.SellRequests.Remove(offer);
                            return true;
                        }
                        else
                        {
                            redSocksRequests.BuyRequests.Add(new Offer() { Name = stockName, SharesNumber = numberOfShares });
                            return false;
                        }
                    }
                case "Blossomers":
                    {
                        var offer = redSocksRequests.SellRequests.Find(r => r.Name == stockName && r.SharesNumber == numberOfShares);
                        if (offer != null)
                        {
                            redSocksRequests.SellRequests.Remove(offer);
                            return true;
                        }
                        else
                        {
                            blossomersRequests.BuyRequests.Add(new Offer() { Name = stockName, SharesNumber = numberOfShares });
                            return false;
                        }
                    }
                default:
                    throw new ArgumentException("Provided buyer " + player.Name + " is not defined.");
            }
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
