using System.Collections.Generic;
using System.Linq;
using RetailEquity.Filters;
using RetailEquity.Model;

namespace RetailEquity.Banks
{
    class BarclaysEngland : IFilter
    {
        public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
        {
            return trades.Where(t => t.Type == TradeType.Future && t.Amount > 100);
        }
    }
}
