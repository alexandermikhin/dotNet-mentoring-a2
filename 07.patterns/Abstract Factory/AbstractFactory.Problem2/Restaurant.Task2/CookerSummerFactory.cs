using System;
using AbstartFactory.Cookers;

namespace AbstartFactory
{
    class CookerSummerFactory: ICookerFactory
    {
        readonly ICooker cooker;

        public CookerSummerFactory(ICooker cooker)
        {
            this.cooker = cooker ?? throw new ArgumentNullException(nameof(cooker));
        }

        public BaseCooker CreateCooker(Country country)
        {
            return country switch
            {
                Country.India => new IndianSummerCooker(cooker),
                Country.Ukraine => new UkranianSummerCooker(cooker),
                Country.England => new EnglandSummerCooker(cooker),
                _ => throw new ArgumentException("There is no cooker for passed country"),
            };
        }
    }
}
