using AbstartFactory;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractFactory.Tests
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
        public void Should_cook_masala(Country country, Ingredient rice, Ingredient chicken)
        {
            masalaCooker.CookMasala(country);

            cooker.Chickens.Count.Should().Be(1);
            cooker.Chickens.First().Should().BeEquivalentTo(chicken);

            cooker.Rices.Count.Should().Be(1);
            cooker.Rices.First().Should().BeEquivalentTo(rice);
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
                    }
                };

                yield return new object[]
                {
                    Country.England,
                    new Ingredient
                    {
                        Amount = 100,
                        FryLevel = Level.Low
                    },
                    new Ingredient
                    {
                        Amount = 100,
                        FryLevel = Level.Low
                    }
                };
            }
        }
    }
}
