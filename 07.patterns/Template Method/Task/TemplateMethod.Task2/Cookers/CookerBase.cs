namespace TemplateMethod.Task2.Cookers
{
    public abstract class CookerBase
    {
        protected abstract void CookRice(ICooker cooker);
        protected abstract void CookChicken(ICooker cooker);
        protected abstract void CookTea(ICooker cooker);

        public void CookMasala(ICooker cooker)
        {
            CookRice(cooker);
            CookChicken(cooker);
            CookTea(cooker);
        }
    }
}
