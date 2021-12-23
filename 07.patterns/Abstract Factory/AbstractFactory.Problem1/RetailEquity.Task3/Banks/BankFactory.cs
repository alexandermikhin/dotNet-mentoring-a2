using System;
using System.Collections.Generic;
using System.Text;
using RetailEquity.Filters;
using RetailEquity.Task3;

namespace RetailEquity.Banks
{
    class BankFactory
    {
        public IFilter CreateBank(Bank bank, Country country)
        {
            return bank switch
            {
                Bank.Bofa => new BofaBank(),
                Bank.Connacord => new ConnacordBank(),
                Bank.Barclays => new Barclays(country),
                Bank.DeutscheBank => new DeutcheBank(),
                _ => throw new ArgumentException(string.Format("Provided bank \"{0}\" is not suppoerted", bank)),
            };
        }
    }
}
