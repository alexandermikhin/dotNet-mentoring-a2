namespace RetailEquity.Model
{
    public class Trade
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public TradeType Type { get; set; }

        public TradeSubType SubType { get; set; }
    }
}
