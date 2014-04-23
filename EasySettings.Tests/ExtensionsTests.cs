using System.Linq;

namespace EasySettings.Tests
{
    using Extensions;

    using NUnit.Framework;

    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void TestPropertyInfo()
        {
            var properties = typeof(TestSettings).GetProperties();
            Assert.AreEqual("My boolean setting", properties.First(x => x.Name == "BooleanValue").GetDescription());
            Assert.AreEqual("", properties.First(x => x.Name == "BooleanStartsTrue").GetDescription());
        }
    }
}
