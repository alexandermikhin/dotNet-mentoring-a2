using AbstartFactory.Cookers;

namespace AbstartFactory
{
    interface ICookerFactory
    {
        BaseCooker CreateCooker(Country country);
    }
}
