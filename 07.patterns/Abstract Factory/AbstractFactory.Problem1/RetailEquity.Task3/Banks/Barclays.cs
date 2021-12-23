using System;
using System.Collections.Generic;
using RetailEquity.Filters;
using RetailEquity.Model;
using RetailEquity.Task3;

namespace RetailEquity.Banks
{
    internal class Barclays : IFilter
    {
        readonly IFilter devision;

        public Barclays(Country country)
        {
            devision = country switch
            {
                Country.USA => new BarclaysUsa(),
                Country.England => new BarclaysEngland(),
                _ => throw new ArgumentException("There is no devision in the provided country " + country),
            };
        }

        public virtual IEnumerable<Trade> Match(IEnumerable<Trade> trades)
        {
            return devision.Match(trades);
        }
    }
}
