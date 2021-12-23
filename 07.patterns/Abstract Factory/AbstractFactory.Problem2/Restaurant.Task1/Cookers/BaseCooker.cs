namespace AbstartFactory.Cookers
{
    abstract class BaseCooker
    {
        protected readonly ICooker cooker;

        public BaseCooker(ICooker cooker)
        {
            this.cooker = cooker;
        }

        public void CookMasala()
        {
            CookRice();
            CookChicken();
        }

        protected virtual void CookRice()
        {
            FryRice();
            SaltRice();
            PepperRice();
        }

        protected virtual void CookChicken()
        {
            FryChicken();
            SaltChicken();
            PepperChicken();
        }

        protected abstract void FryRice();
        protected abstract void SaltRice();
        protected abstract void PepperRice();
        protected abstract void FryChicken();
        protected abstract void SaltChicken();
        protected abstract void PepperChicken();
    }
}
