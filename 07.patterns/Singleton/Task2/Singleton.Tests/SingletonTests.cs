using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Singleton.Tests
{
    [TestClass]
    public class SingletonTests
    {
        [TestMethod]
        public void Should_create_only_one_instance()
        {
            var instance1 = ThreadSafe.Singleton.Instance;
            var instance2 = ThreadSafe.Singleton.Instance;
            var instance3 = ThreadSafe.Singleton.Instance;

            Assert.AreEqual(instance1, instance2);
            Assert.AreEqual(instance2, instance3);
        }
    }
}
