using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySettings.Tests.Storage
{
    using System.Web;

    using NUnit.Framework;

    using SettingsStorage;

    [TestFixture]
    public class HttpContextStorageTests
    {
        [Test]
        public void TestHttpContextStorage()
        {
            var context = new HttpContext(new HttpRequest(null, "http://tempuri.org", null), new HttpResponse(null));

            var storage = new HttpContextSettingsStorage(new HttpApplicationStateWrapper(context.Application));

            Assert.AreEqual(false, storage.GetAllValues().Any());
            storage.SaveSetting("MyNewSetting", "true");

            Assert.AreEqual("true", storage.GetValue("MyNewSetting"));
            Assert.AreEqual("", storage.GetValue("SomethingThatShouldntExist"));
        }
    }
}
