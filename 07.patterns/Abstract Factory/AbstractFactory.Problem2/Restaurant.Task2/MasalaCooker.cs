using System;

namespace AbstartFactory
{
    public class MasalaCooker
    {
        private readonly ICooker cooker;

        public MasalaCooker(ICooker cooker)
        {
            this.cooker = cooker;
        }

        public void CookMasala(DateTime currentDate, Country country)
        {
            ICookerFactory factory;
            if (IsSummerDate(currentDate))
            {
                factory = new CookerSummerFactory(cooker);
            }
            else
            {
                factory = new CookerFactory(cooker);
            }

            var countryCooker = factory.CreateCooker(country);
            countryCooker.CookMasala();
        }

        private bool IsSummerDate(DateTime currentDate)
        {
            return currentDate.Month >= 6 && currentDate.Month <= 8;
        }
    }
}
