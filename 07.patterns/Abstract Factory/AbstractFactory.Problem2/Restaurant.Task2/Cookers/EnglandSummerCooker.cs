namespace AbstartFactory.Cookers
{
    class EnglandSummerCooker: BaseCooker
    {
        public EnglandSummerCooker(ICooker cooker) : base(cooker)
        { }

        protected override void FryChicken()
        {
            cooker.FryChicken(50, Level.Low);
        }

        protected override void FryRice()
        {
            cooker.FryRice(50, Level.Low);
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
