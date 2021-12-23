using System.Collections.Generic;
using System.Linq;
using RetailEquity.Filters;
using RetailEquity.Model;

namespace RetailEquity.Task1.Banks
{
    internal class BofaBank : IFilter
    {
        public IEnumerable<Trade> Match(IEnumerable<Trade> trades)
        {
            return trades.Where(t => t.Amount > 70);
        }
    }
}
