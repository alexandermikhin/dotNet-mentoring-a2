using AbstartFactory;

namespace AbstartFactory.Cookers
{
    class IndianCooker : BaseCooker
    {
        public IndianCooker(ICooker cooker): base(cooker)
        { }

        protected override void FryChicken()
        {
            cooker.FryChicken(100, Level.Strong);
        }

        protected override void FryRice()
        {
            cooker.FryRice(200, Level.Strong);
        }

        protected override void PepperChicken()
        {
            cooker.PepperChicken(Level.Strong);
        }

        protected override void PepperRice()
        {
            cooker.PepperRice(Level.Strong);
        }

        protected override void SaltChicken()
        {
            cooker.SaltChicken(Level.Strong);
        }

        protected override void SaltRice()
        {
            cooker.SaltRice(Level.Strong);
        }
    }
}
