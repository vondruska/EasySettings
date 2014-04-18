namespace EasySettings.Tests
{
    using EasySettings.Storage;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTests
    {
        [TestInitialize]
        public void Init()
        {
            Configuration.PersistantSettingsProvider = new LocalStorage();
        }


        [TestMethod]
        public void TestDefaultBehavior()
        {
            Assert.AreEqual(default(bool), Current<TestSettings>.Settings.BooleanValue);
            Assert.AreEqual(true, Current<TestSettings>.Settings.BooleanStartsTrue);
            Assert.AreEqual(MySettingsEnum.DefaultValue, Current<TestSettings>.Settings.EnumValue);
            Assert.AreEqual(default(int), Current<TestSettings>.Settings.IntegerValue);
            Assert.AreEqual(default(string), Current<TestSettings>.Settings.StringValue);

            Assert.AreEqual(0, Configuration.PersistantSettingsProvider.GetAllValues().Count);
        }

        [TestMethod]
        public void TestSetSettingsDirectlyOnStorage()
        {
            Configuration.PersistantSettingsProvider.SaveSetting("BooleanValue", "true");
            Assert.AreEqual(true, bool.Parse(Configuration.PersistantSettingsProvider.GetValue("BooleanValue").ToString()));
            Assert.AreEqual(true, Current<TestSettings>.Settings.BooleanValue);
        }

        [TestMethod]
        public void TestInflator()
        {
            var inflator = Inflator<TestSettings>.InflateSettings();
            Assert.AreEqual(false, inflator.BooleanValue);
            Assert.AreEqual(true, inflator.BooleanStartsTrue);
            Assert.AreEqual(MySettingsEnum.DefaultValue, inflator.EnumValue);
            Assert.AreEqual(default(int), inflator.IntegerValue);
            Assert.AreEqual(default(string), inflator.StringValue);

            Configuration.PersistantSettingsProvider.SaveSetting("BooleanValue", "true");
            Configuration.PersistantSettingsProvider.SaveSetting("BooleanStartsTrue", "false");
            Configuration.PersistantSettingsProvider.SaveSetting("EnumValue", "FirstValue");
            Configuration.PersistantSettingsProvider.SaveSetting("IntegerValue", "1000");
            Configuration.PersistantSettingsProvider.SaveSetting("StringValue", "Lipsum");
            

            inflator = Inflator<TestSettings>.InflateSettings();
            Assert.AreEqual(true, inflator.BooleanValue);
            Assert.AreEqual(false, inflator.BooleanStartsTrue);
            Assert.AreEqual(MySettingsEnum.FirstValue, inflator.EnumValue);
            Assert.AreEqual(1000, inflator.IntegerValue);
            Assert.AreEqual("Lipsum", inflator.StringValue);
        }

        [TestMethod]
        public void TestDisabling()
        {
            Assert.AreEqual(true, Current<TestSettings>.Settings.BooleanStartsTrue);
            
            Configuration.Enabled = false;
            
            Configuration.PersistantSettingsProvider.SaveSetting("BooleanStartsTrue", "false");

            Assert.AreEqual(true, Current<TestSettings>.Settings.BooleanStartsTrue);
        }

    }
}
