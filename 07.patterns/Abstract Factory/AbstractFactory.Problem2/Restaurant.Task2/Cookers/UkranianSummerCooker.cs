namespace AbstartFactory.Cookers
{
    class UkranianSummerCooker: BaseCooker
    {
        public UkranianSummerCooker(ICooker cooker) : base(cooker)
        { }

        protected override void FryChicken()
        {
            cooker.FryChicken(200, Level.Medium);
        }

        protected override void FryRice()
        {
            cooker.FryRice(150, Level.Medium);
        }

        protected override void PepperChicken()
        {
        }

        protected override void PepperRice()
        {
        }

        protected override void SaltChicken()
        {
            cooker.SaltChicken(Level.Low);
        }

        protected override void SaltRice()
        {
            cooker.SaltRice(Level.Low);
        }
    }
}
