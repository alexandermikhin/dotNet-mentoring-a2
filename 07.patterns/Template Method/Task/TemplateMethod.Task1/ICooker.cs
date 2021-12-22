namespace TemplateMethod.Task1
{
    public interface ICooker
    {
        void FryRice(int amount, Level level);
        void FryChicken(int amount, Level level);
        void SaltRice(Level level);
        void SaltChicken(Level level);
        void PepperRice(Level level);
        void PepperChicken(Level level);
        void PrepareTea(int teaGram, TeaKind teaType, int honeyGram);
    }
}
