namespace EasySettings
{
    using ObjectCaching;

    using SettingsStorage;

    public static class Configuration
    {
        static Configuration()
        {
            SettingsProvider = new LocalSettingsStorage();
            SettingsObjectCache = new StandardObjectCache();
        }

        public static ISettingsStorage SettingsProvider { get; set; }

        internal static ISettingsObjectCache SettingsObjectCache { get; set; }
    }
}
