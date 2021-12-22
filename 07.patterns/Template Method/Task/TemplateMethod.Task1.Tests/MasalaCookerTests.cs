using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TemplateMethod.Task1.Tests
{
    [TestClass]
    public class MasalaCookerTests
    {
        private FakeCooker cooker;
        private MasalaCooker masalaCooker;

        public MasalaCookerTests()
        {
            cooker = new FakeCooker();
            masalaCooker = new MasalaCooker(cooker);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetMasalaData), DynamicDataSourceType.Property)]
        public void Should_cook_masala(Country country, Ingredient rice, Ingredient chicken, TeaIngredient tea)
        {
            masalaCooker.CookMasala(country);

            cooker.Chickens.Count.Should().Be(1);
            cooker.Chickens.First().Should().BeEquivalentTo(chicken);

            cooker.Rices.Count.Should().Be(1);
            cooker.Rices.First().Should().BeEquivalentTo(rice);

            cooker.Teas.Count.Should().Be(1);
            cooker.Teas.First().Should().BeEquivalentTo(tea);
        }

        public static IEnumerable<object[]> GetMasalaData 
        {
            get
            {
                yield return new object[]
                {
                    Country.India,
                    new Ingredient
                    {
                        Amount = 200,
                        FryLevel = Level.Strong,
                        PepperLevel = Level.Strong,
                        SaltLevel = Level.Strong
                    },
                    new Ingredient
                    {
                        Amount = 100,
                        FryLevel = Level.Strong,
                        PepperLevel = Level.Strong,
                        SaltLevel = Level.Strong
                    },
                    new TeaIngredient
                    { 
                        HoneyGram = 12,
                        TeaGram = 15,
                        TeaType = TeaKind.Green
                    }
                };

                yield return new object[]
                {
                    Country.Ukraine,
                    new Ingredient
                    {
                        Amount = 500,
                        FryLevel = Level.Strong,
                        PepperLevel = Level.Low,
                        SaltLevel = Level.Strong
                    },
                    new Ingredient
                    {
                        Amount = 300,
                        FryLevel = Level.Medium,
                        PepperLevel = Level.Low,
                        SaltLevel = Level.Medium
                    },
                    new TeaIngredient
                    {
                        HoneyGram = 10,
                        TeaGram = 10,
                        TeaType = TeaKind.Black
                    }
                };
            }
        }
    }
}
