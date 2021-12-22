using System;
using TemplateMethod.Task2.Cookers;

namespace TemplateMethod.Task2
{
    public class MasalaCooker
    {
        private ICooker cooker;

        public MasalaCooker(ICooker cooker)
        {
            this.cooker = cooker;
        }

        public void CookMasala(Country country)
        {
            CookerBase baseCooker = null;
            switch (country)
            {
                case Country.India:
                    baseCooker = new IndiaCooker();
                    break;
                case Country.Ukraine:
                    baseCooker = new UkraineCooker();
                    break;
            }

            if (baseCooker != null)
            {
                baseCooker.CookMasala(cooker);
            }
        }
    }
}
