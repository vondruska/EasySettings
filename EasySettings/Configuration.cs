namespace EasySettings
{
    using Storage;

    public static class Configuration
    {
        static Configuration()
        {
            PersistantSettingsProvider = new HttpContextStorage();
            Enabled = true;
        }

        public static IStorage PersistantSettingsProvider { get; set; }

        public static bool Enabled { get; set; }
    }
}
