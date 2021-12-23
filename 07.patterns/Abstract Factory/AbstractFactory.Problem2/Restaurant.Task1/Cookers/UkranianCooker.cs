using AbstartFactory;

namespace AbstartFactory.Cookers
{
    class UkranianCooker: BaseCooker
    {
        public UkranianCooker(ICooker cooker) : base(cooker)
        { }

        protected override void FryChicken()
        {
            cooker.FryChicken(300, Level.Medium);
        }

        protected override void FryRice()
        {
            cooker.FryRice(500, Level.Strong);
        }

        protected override void PepperChicken()
        {
            cooker.PepperChicken(Level.Low);
        }

        protected override void PepperRice()
        {
            cooker.PepperRice(Level.Low);
        }

        protected override void SaltChicken()
        {
            cooker.SaltChicken(Level.Medium);
        }

        protected override void SaltRice()
        {
            cooker.SaltRice(Level.Strong);
        }
    }
}
