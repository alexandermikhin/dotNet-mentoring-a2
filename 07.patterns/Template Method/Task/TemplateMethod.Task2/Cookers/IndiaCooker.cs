namespace TemplateMethod.Task2.Cookers
{
    public class IndiaCooker : CookerBase
    {
        protected override void CookRice(ICooker cooker)
        {
            cooker.FryRice(200, Level.Strong);
            cooker.SaltRice(Level.Strong);
            cooker.PepperRice(Level.Strong);
        }

        protected override void CookChicken(ICooker cooker)
        {
            cooker.FryChicken(100, Level.Strong);
            cooker.SaltChicken(Level.Strong);
            cooker.PepperChicken(Level.Strong);
        }

        protected override void CookTea(ICooker cooker)
        {
            cooker.PrepareTea(15, TeaKind.Green, 12);
        }
    }
}
