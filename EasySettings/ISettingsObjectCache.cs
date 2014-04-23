namespace EasySettings
{
    public interface ISettingsObjectCache
    {
        /// <summary>
        /// Gets or sets the current settings object to be cached
        /// </summary>
        object CurrentSettingsObject { get; set; }

        /// <summary>
        /// Resets/removes the settings object from the cache
        /// </summary>
        void Reset();
    }
}
