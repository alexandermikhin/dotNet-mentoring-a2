using System.Collections.Generic;
using System.Linq;
using RetailEquity.Filters;
using RetailEquity.Model;

namespace RetailEquity.Banks
{
    internal class Barclays : IFilter
    {
        public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
        {
            return trades.Where(t => t.Type == TradeType.Option && t.SubType == TradeSubType.NyOption && t.Amount > 50);
        }
    }
}
