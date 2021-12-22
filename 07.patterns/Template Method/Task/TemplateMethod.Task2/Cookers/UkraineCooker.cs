namespace TemplateMethod.Task2.Cookers
{
    public class UkraineCooker : CookerBase
    {
        protected override void CookRice(ICooker cooker)
        {
            cooker.FryRice(500, Level.Strong);
            cooker.SaltRice(Level.Strong);
            cooker.PepperRice(Level.Low);
        }

        protected override void CookChicken(ICooker cooker)
        {
            cooker.FryChicken(300, Level.Medium);
            cooker.SaltChicken(Level.Medium);
            cooker.PepperChicken(Level.Low);
        }

        protected override void CookTea(ICooker cooker)
        {
            cooker.PrepareTea(10, TeaKind.Black, 10);
        }
    }
}
