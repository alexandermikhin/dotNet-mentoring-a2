using System.Collections.Generic;
using RetailEquity.Model;
using RetailEquity.Task1.Banks;

namespace RetailEquity
{
    public class TradeFilter
    {
        BankFactory factory = new BankFactory();
        public IEnumerable<Trade> FilterForBank(IEnumerable<Trade> trades, Bank bank)
        {
            var filter = factory.CreateBank(bank);
            return filter.Match(trades);
        }
    }
}