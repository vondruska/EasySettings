namespace EasySettings.Tests
{
    using NUnit.Framework;

    using SettingsStorage;

    [TestFixture]
    public class UnitTests
    {
        [SetUp]
        public void Init()
        {
            Configuration.SettingsProvider = new LocalSettingsStorage();
        }

        [Test]
        public void TestDefaultBehavior()
        {
            Assert.AreEqual(default(bool), Settings.Current<TestSettings>().BooleanValue);
            Assert.AreEqual(true, Settings.Current<TestSettings>().BooleanStartsTrue);
            Assert.AreEqual(MySettingsEnum.DefaultValue, Settings.Current<TestSettings>().EnumValue);
            Assert.AreEqual(default(int), Settings.Current<TestSettings>().IntegerValue);
            Assert.AreEqual(default(string), Settings.Current<TestSettings>().StringValue);

            Assert.AreEqual(0, Configuration.SettingsProvider.GetAllValues().Count);
        }

        [Test]
        public void TestInflator()
        {
            var inflator = new Binder().Bind<TestSettings>();
            Assert.AreEqual(false, inflator.BooleanValue);
            Assert.AreEqual(true, inflator.BooleanStartsTrue);
            Assert.AreEqual(MySettingsEnum.DefaultValue, inflator.EnumValue);
            Assert.AreEqual(default(int), inflator.IntegerValue);
            Assert.AreEqual(default(string), inflator.StringValue);

            Configuration.SettingsProvider.SaveSetting("BooleanValue", "true");
            Configuration.SettingsProvider.SaveSetting("BooleanStartsTrue", "false");
            Configuration.SettingsProvider.SaveSetting("EnumValue", "FirstValue");
            Configuration.SettingsProvider.SaveSetting("IntegerValue", "1000");
            Configuration.SettingsProvider.SaveSetting("StringValue", "Lipsum");
            

            inflator = new Binder().Bind<TestSettings>();
            Assert.AreEqual(true, inflator.BooleanValue);
            Assert.AreEqual(false, inflator.BooleanStartsTrue);
            Assert.AreEqual(MySettingsEnum.FirstValue, inflator.EnumValue);
            Assert.AreEqual(1000, inflator.IntegerValue);
            Assert.AreEqual("Lipsum", inflator.StringValue);
        }

        [Test]
        public void TestDynamic()
        {
            Assert.AreEqual(false, Settings.Current().BooleanValue);
            Assert.AreEqual(false, Settings.Current<TestSettings>().BooleanValue);

            Configuration.SettingsProvider.SaveSetting("BooleanValue", "true");

            Assert.AreEqual(true, Settings.Current().BooleanValue);
            Assert.AreEqual(true, Settings.Current<TestSettings>().BooleanValue);
        }

    }
}
