using AbstartFactory;

namespace AbstartFactory.Cookers
{
    class EnglandCooker: BaseCooker
    {
        public EnglandCooker(ICooker cooker) : base(cooker)
        { }

        protected override void FryChicken()
        {
            cooker.FryChicken(100, Level.Low);
        }

        protected override void FryRice()
        {
            cooker.FryRice(100, Level.Low);
        }

        protected override void PepperChicken()
        {
        }

        protected override void PepperRice()
        {
        }

        protected override void SaltChicken()
        {
        }

        protected override void SaltRice()
        {
        }
    }
}
