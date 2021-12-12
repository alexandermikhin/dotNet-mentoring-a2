using System;
using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        Mapper<Foo, Bar> _mapper;

        [TestInitialize]
        public void InitTest()
        {
            var mapGenerator = new MappingGenerator();
            _mapper = mapGenerator.Generate<Foo, Bar>();
        }

        [TestMethod]
        public void Map_CreatesInstanceOfNewObject()
        {
            var res = _mapper.Map(new Foo());
            Assert.IsNotNull(res);
            Assert.AreEqual(typeof(Bar), res.GetType());
        }

        [TestMethod]
        public void Map_ShouldSetPublicProperties_IfMatch()
        {
            var foo = new Foo() { Id = 123 };
            var res = _mapper.Map(foo);
            Assert.AreEqual(123, res.Id);
        }

        [TestMethod]
        public void Map_ShouldSetPublicFields_IfMatch()
        {
            var foo = new Foo() { commonField = "field" };
            var res = _mapper.Map(foo);
            Assert.AreEqual("field", res.commonField);
        }

        [TestMethod]
        public void Map_NotSetPublicProperties_OfDifferentTypes()
        {
            var foo = new Foo() { Date = new DateTime(2021, 12, 10) };
            var res = _mapper.Map(foo);
            Assert.AreEqual(default, res.Date);
        }

        [TestMethod]
        public void Map_NotSetPublicProperties_IfSourceOnlySet_AndDestinationOnlyGet()
        {
            var foo = new Foo() { Year = 123 };
            var res = _mapper.Map(foo);
            Assert.AreEqual(default, res.Year);
        }

        [TestMethod]
        public void Map_IgnoresProperties_WhichAreMarkedIgnored()
        {
            var foo = new Foo() { FooIgnored = "foo-ignored", BarIgnored = "bar-ignored" };
            var res = _mapper.Map(foo);
            Assert.AreEqual(default, res.BarIgnored);
            Assert.AreEqual(default, res.FooIgnored);
        }
    }
}
