namespace EasySettings.ObjectCaching
{
    using System.Web;

    /// <summary>
    /// The standard bound and inflated object storage
    /// </summary>
    internal class StandardObjectCache : ISettingsObjectCache
    {
        const string Key = "!TheSettingsObject!";

        static object SettingsObject { get; set; }

        /// <summary>
        /// Gets or sets the settings object from the either the static storage, or HttpContext.Items
        /// </summary>
        public object CurrentSettingsObject
        {
            get
            {
                var context = HttpContext.Current;
                return context == null ? SettingsObject : context.Items[Key];
            }
            set
            {
                var context = HttpContext.Current;

                if (context == null) SettingsObject = value;
                else context.Items[Key] = value;
            }
        }

        /// <summary>
        /// Removes the object from the cache storage and forces a recalculation
        /// </summary>
        public void Reset()
        {
            var context = HttpContext.Current;
            if (context == null) SettingsObject = null;
            else context.Items[Key] = null;
        }
    }
}
