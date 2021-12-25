using FeedManager.Task1.FeedImporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractFactory.Tests
{
    [TestClass]
    public class Delta1FeedValidatorTests
    {
        private Delta1FeedValidator feedValidator;

        public Delta1FeedValidatorTests()
        {
            feedValidator = new Delta1FeedValidator();
        }
    }
}
