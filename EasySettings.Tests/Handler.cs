namespace EasySettings.Tests.Web
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    using EasySettings.Web;

    using NUnit.Framework;

    using Subtext.TestLibrary;

    [TestFixture]
    public class Handler
    {
        [Test]
        public void TestInflator()
        {
            var handler = new EasySettingsHandler();
            var testSettings = new TestSettings();
            var result = handler.InflateSettingsViewModel(new SettingsClassHelper(testSettings));

            Assert.AreEqual(5, result.Count());

            //TODO: assure the viewmodel are the same
        }
        
        [Test]
        public void TestGetRequest()
        {
            var helper = new SettingsClassHelper(new TestSettings());
            var cookies = new HttpCookieCollection();
            var response = new EasySettingsHandler().ProcessGet(helper, cookies);

            Assert.IsTrue(cookies.Count == 1);
            Assert.IsTrue(response.Contains(cookies[0].Value));
        }

        [Test]
        public void TestPostRequest()
        {
            var helper = new SettingsClassHelper(new TestSettings());
            var cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("easysettings-token", "6d5f6e66bf0e4a638490abfb073d0e68"));

            byte[] byteArray = Encoding.UTF8.GetBytes(@"{""settings"":[{""type"":0,""name"":""IntegerValue"",""value"":""0"",""desc"":""This is an integer value with a really long description that should wrap eventually"",""possibleValues"":null},{""type"":1,""name"":""MiniProfiler"",""value"":true,""desc"":""Allow MiniProfiler to run"",""possibleValues"":null},{""type"":1,""name"":""MyTest"",""value"":false,""desc"":"""",""possibleValues"":null},{""type"":2,""name"":""MyTestEnum"",""value"":""ThisIs0"",""desc"":"""",""possibleValues"":[""ThisIs0"",""ThisIs1""]}],""token"":""6d5f6e66bf0e4a638490abfb073d0e68""}");
            var stream = new MemoryStream(byteArray);
            var response = new EasySettingsHandler().ProcessPost(stream, helper, cookies);


        }
    }
}
