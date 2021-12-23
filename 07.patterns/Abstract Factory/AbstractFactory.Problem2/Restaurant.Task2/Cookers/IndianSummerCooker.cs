namespace AbstartFactory.Cookers
{
    class IndianSummerCooker: BaseCooker
    {
        public IndianSummerCooker(ICooker cooker) : base(cooker)
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
            cooker.PepperChicken(Level.Medium);
        }

        protected override void PepperRice()
        {
            cooker.PepperRice(Level.Medium);
        }

        protected override void SaltChicken()
        {
        }

        protected override void SaltRice()
        {
        }
    }
}
