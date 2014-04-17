namespace EasySettings
{
    using Cache;

    using Storage;

    public static class Configuration
    {
        static Configuration()
        {
            PersistantSettingsProvider = new HttpContextStorage();
            CacheProvider = new HttpRuntimeCache();
            Enabled = true;
        }

        public static IStorage PersistantSettingsProvider { get; set; }

        public static ICache CacheProvider { get; set; }

        public static bool Enabled { get; set; }
    }
}
