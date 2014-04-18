using System.Linq;

namespace EasySettings.Tests
{
    using Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void TestPropertyInfo()
        {
            var properties = typeof(TestSettings).GetProperties();
            Assert.AreEqual("My boolean setting", properties.First(x => x.Name == "BooleanValue").GetDescription());
            Assert.AreEqual("", properties.First(x => x.Name == "BooleanStartsTrue").GetDescription());
        }
    }
}
