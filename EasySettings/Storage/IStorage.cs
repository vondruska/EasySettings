namespace EasySettings.Storage
{
    using System.Collections.Generic;

    public interface IStorage
    {
        /// <summary>
        /// Save a setting to storage
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value.</param>
        void SaveSetting(string key, string value);

        /// <summary>
        /// Get a value from the storage provider
        /// </summary>
        /// <param name="key">The unique key</param>
        /// <returns>Object value</returns>
        object GetValue(string key);

        /// <summary>
        /// Get all the settings from this provider
        /// </summary>
        /// <returns>enumerable list of key/value setting pairs</returns>
        Dictionary<string, string> GetAllValues();

        /// <summary>
        /// Called on application startup to ensure the storage provider is ready to provide settings
        /// </summary>
        void Initialize();
    }
}
