namespace EasySettings.SettingsStorage
{
    using System.Collections.Generic;

    public class LocalSettingsStorage : ISettingsStorage
    {
        private readonly Dictionary<string, string> _storage = new Dictionary<string, string>();

        public void SaveSetting(string key, string value)
        {
            _storage[key] = value;
        }

        public string GetValue(string key)
        {
            return _storage[key];
        }

        public Dictionary<string, string> GetAllValues()
        {
            return _storage;
        }

        public void Initialize()
        {}
    }
}
