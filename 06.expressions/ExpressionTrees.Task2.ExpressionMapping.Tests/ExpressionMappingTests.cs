using System;
using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void Map_CreatesInstanceOfNewObject()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var res = mapper.Map(new Foo());
            Assert.IsNotNull(res);
            Assert.AreEqual(typeof(Bar), res.GetType());
        }

        [TestMethod]
        public void Map_ShouldSetPublicProperties_IfMatch()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();
            var foo = new Foo() { Id = 123 };
            var res = mapper.Map(foo);
            Assert.AreEqual(123, res.Id);
        }

        [TestMethod]
        public void Map_ShouldSetPublicFields_IfMatch()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();
            var foo = new Foo() { commonField = "field" };
            var res = mapper.Map(foo);
            Assert.AreEqual("field", res.commonField);
        }

        [TestMethod]
        public void Map_NotSetPublicProperties_OfDifferentTypes()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();
            var foo = new Foo() { Date = new DateTime(2021, 12, 10) };
            var res = mapper.Map(foo);
            Assert.AreEqual(default, res.Date);
        }

        [TestMethod]
        public void Map_NotSetPublicProperties_IfSourceOnlySet_AndDestinationOnlyGet()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();
            var foo = new Foo() { Year = 123 };
            var res = mapper.Map(foo);
            Assert.AreEqual(default, res.Year);
        }
    }
}
