namespace EasySettings
{
    using System.Reflection;
    using System.Web;
    using Dynamic;

    public class Settings
    {
        /// <summary>
        /// Gets the current instance of the settings
        /// </summary>
        /// <typeparam name="T">The settings class that inherits BaseEasySettings</typeparam>
        /// <returns>An instance of T with configured settings</returns>
        public static T Current<T>()
            where T : class
        {
            if (Configuration.SettingsObjectCache.CurrentSettingsObject == null)
            {
                Configuration.SettingsObjectCache.CurrentSettingsObject = new Binder().Bind<T>();
            }

            return (T)Configuration.SettingsObjectCache.CurrentSettingsObject;
        }

        /// <summary>
        /// Gets a dynamic instance of the settings
        /// </summary>
        /// <returns>Dynamic object with a collection of settings from the class that inherits BaseEasySettings</returns>
        public static dynamic Current()
        {
            if (HttpContext.Current != null)
            {
                var binder = new Binder().Bind(new SettingsClassHelper(HttpContext.Current).TheClass);
                return new ReadOnlyDynamicDictonary(binder);
            }
            else
            {
                var binder = new Binder().Bind(new SettingsClassHelper(Assembly.GetCallingAssembly()).TheClass);
                return new ReadOnlyDynamicDictonary(binder);
            }
        }
    }
}
