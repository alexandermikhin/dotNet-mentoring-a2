using AbstartFactory;

namespace AbstractFactory.Tests
{
    public class Ingredient
    {
        public int Amount { get; set; }
        public Level FryLevel { get; set; }
        public Level? PepperLevel { get; set; }
        public Level? SaltLevel { get; set; }
    }
}
