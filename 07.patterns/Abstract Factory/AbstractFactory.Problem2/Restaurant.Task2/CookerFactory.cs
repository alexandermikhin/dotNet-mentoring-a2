using System;
using AbstartFactory.Cookers;

namespace AbstartFactory
{
    class CookerFactory: ICookerFactory
    {
        readonly ICooker cooker;

        public CookerFactory(ICooker cooker)
        {
            this.cooker = cooker ?? throw new ArgumentNullException(nameof(cooker));
        }

        public BaseCooker CreateCooker(Country country)
        {
            return country switch
            {
                Country.India => new IndianCooker(cooker),
                Country.Ukraine => new UkranianCooker(cooker),
                Country.England => new EnglandCooker(cooker),
                _ => throw new ArgumentException("There is no cooker for passed country"),
            };
        }
    }
}
