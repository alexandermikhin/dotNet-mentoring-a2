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
        public void Should_cook_masala(DateTime currentDate, Country country, Ingredient rice, Ingredient chicken)
        {
            masalaCooker.CookMasala(currentDate, country);

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
                    DateTime.Parse("2/16/2020"),
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
                    DateTime.Parse("2/16/2020"),
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
                    DateTime.Parse("2/16/2020"),
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

                yield return new object[]
                {
                    DateTime.Parse("7/20/2020"),
                    Country.India,
                    new Ingredient
                    {
                        Amount = 100,
                        FryLevel = Level.Low,
                        PepperLevel = Level.Medium
                    },
                    new Ingredient
                    {
                        Amount = 100,
                        FryLevel = Level.Low,
                        PepperLevel = Level.Medium
                    }
                };

                yield return new object[]
                {
                    DateTime.Parse("7/20/2020"),
                    Country.Ukraine,
                    new Ingredient
                    {
                        Amount = 150,
                        FryLevel = Level.Medium,
                        SaltLevel = Level.Low
                    },
                    new Ingredient
                    {
                        Amount = 200,
                        FryLevel = Level.Medium,
                        SaltLevel = Level.Low
                    }
                };

                yield return new object[]
                {
                    DateTime.Parse("7/20/2020"),
                    Country.England,
                    new Ingredient
                    {
                        Amount = 50,
                        FryLevel = Level.Low
                    },
                    new Ingredient
                    {
                        Amount = 50,
                        FryLevel = Level.Low
                    }
                };
            }
        }
    }
}
