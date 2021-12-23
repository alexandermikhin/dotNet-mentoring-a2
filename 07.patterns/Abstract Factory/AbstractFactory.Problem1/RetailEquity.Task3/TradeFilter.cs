using System.Collections.Generic;
using RetailEquity.Banks;
using RetailEquity.Model;
using RetailEquity.Task3;

namespace RetailEquity
{
    public class TradeFilter
    {
        readonly BankFactory factory = new BankFactory();

        public IEnumerable<Trade> FilterForBank(IEnumerable<Trade> trades, Bank bank, Country country)
        {
            var filter = factory.CreateBank(bank, country);
            return filter.Match(trades);
        }
    }
}