using RetailEquity.Model;
using System.Collections.Generic;

namespace RetailEquity.Filters
{
    public interface IFilter
    {
        IEnumerable<Trade> Match(IEnumerable<Trade> trades);
    }
}
