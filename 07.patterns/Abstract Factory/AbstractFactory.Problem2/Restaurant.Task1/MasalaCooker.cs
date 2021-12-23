namespace AbstartFactory
{
    public class MasalaCooker
    {
        private readonly ICooker cooker;
        private readonly CookerFactory cookerFactory;

        public MasalaCooker(ICooker cooker)
        {
            this.cooker = cooker;
            cookerFactory = new CookerFactory(this.cooker);
        }

        public void CookMasala(Country country)
        {
            var usedCooler = cookerFactory.CreateCooker(country);
            usedCooler.CookMasala();
        }
    }
}
