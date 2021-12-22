namespace TemplateMethod.Task1
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
            CookRice(country);
            CookChicken(country);
            CookTea(country);
        }

        private void CookRice(Country country)
        {
            switch (country)
            {
                case Country.India:
                    cooker.FryRice(200, Level.Strong);
                    cooker.SaltRice(Level.Strong);
                    cooker.PepperRice(Level.Strong);
                    break;
                case Country.Ukraine:
                    cooker.FryRice(500, Level.Strong);
                    cooker.SaltRice(Level.Strong);
                    cooker.PepperRice(Level.Low);
                    break;
            }
        }

        private void CookChicken(Country country)
        {
            switch (country)
            {
                case Country.India:
                    cooker.FryChicken(100, Level.Strong);
                    cooker.SaltChicken(Level.Strong);
                    cooker.PepperChicken(Level.Strong);
                    break;
                case Country.Ukraine:
                    cooker.FryChicken(300, Level.Medium);
                    cooker.SaltChicken(Level.Medium);
                    cooker.PepperChicken(Level.Low);
                    break;
            }
        }

        private void CookTea(Country country)
        {
            switch (country)
            {
                case Country.India:
                    cooker.PrepareTea(15, TeaKind.Green, 12);
                    break;
                case Country.Ukraine:
                    cooker.PrepareTea(10, TeaKind.Black, 10);
                    break;
            }
        }
    }
}
