namespace EasySettings.Cache
{
    using System.Web;

    class HttpRuntimeCache : ICache
    {
        const string CacheString = "EasySettingsCache";

        public void Store(object obj)
        {
            HttpRuntime.Cache.Insert(CacheString, obj);
        }

        public void Clear()
        {
            HttpRuntime.Cache.Remove(CacheString);
        }

        public object GetObject()
        {
            return HttpRuntime.Cache[CacheString];
        }
    }
}
