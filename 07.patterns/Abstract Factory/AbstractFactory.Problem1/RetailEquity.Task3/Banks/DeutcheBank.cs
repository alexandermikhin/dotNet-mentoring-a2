using System.Collections.Generic;
using System.Linq;
using RetailEquity.Filters;
using RetailEquity.Model;

namespace RetailEquity.Banks
{
    internal class DeutcheBank : IFilter
    {
        public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
        {
            return trades.Where(t => t.Type == TradeType.Option
                && t.SubType == TradeSubType.NewOption
                && t.Amount > 90
                && t.Amount < 120);
        }
    }
}
