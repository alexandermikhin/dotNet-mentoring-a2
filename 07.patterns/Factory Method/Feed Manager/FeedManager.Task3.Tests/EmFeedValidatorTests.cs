using FeedManager.Task1.FeedImporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FeedManager.Tests.Task1.Tests
{
    [TestClass]
    public class EmFeedValidatorTests
    {
        private EmFeedValidator feedValidator;

        public EmFeedValidatorTests()
        {
            feedValidator = new EmFeedValidator();
        }
    }
}
