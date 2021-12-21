using System;

namespace StockExchange.Task4
{
    public class DealEventArgs: EventArgs
    {
        public string BuyerName { get; set; }
        public string SellerName { get; set; }
        public int SharesNumber { get; set; }
    }
}
