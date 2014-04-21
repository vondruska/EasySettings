namespace EasySettings.Tests.Storage
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Web;

    using EasySettings.Storage;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HttpSessionStorageTests
    {
        [TestMethod]
        public void TestHttpSessionStorage()
        {
            var storage = new HttpSessionStorage(new HttpSessionMock());
            Assert.AreEqual(false, storage.GetAllValues().Any());
            storage.SaveSetting("MyNewSetting", "true");

            Assert.AreEqual("true", storage.GetValue("MyNewSetting"));
            Assert.AreEqual("", storage.GetValue("SomethingThatShouldntExist"));

        }
    }

    public class HttpSessionMock : HttpSessionStateBase
    {
        private readonly NameValueCollection _keyCollection = new NameValueCollection();
        private readonly Dictionary<string, object> _objects = new Dictionary<string, object>();

        public override object this[string name]
        {
            get
            {
                object result = null;

                if (_objects.ContainsKey(name))
                {
                    result = _objects[name];
                }

                return result;

            }
            set
            {
                _objects[name] = value;
                _keyCollection[name] = null;
            }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get { return _keyCollection.Keys; }
        }
    }
}
