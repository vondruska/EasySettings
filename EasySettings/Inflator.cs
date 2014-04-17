namespace EasySettings
{
    using System;
    using System.ComponentModel;

    internal static class Inflator
    {
        internal static T InflateSettings<T>()
            where T : class
        {
            var cacheItem = Configuration.CacheProvider.GetObject();
            if (cacheItem != null)
            {
                var returnItem = cacheItem as T;
                if (returnItem != null) return returnItem;
            }

            var theClass = Activator.CreateInstance<T>();
            var storedValues = Configuration.PersistantSettingsProvider.GetAllValues();

            foreach (var item in storedValues)
            {
                var property = theClass.GetType().GetProperty(item.Key);
                if (property == null || !property.CanWrite) continue;

                var foo = TypeDescriptor.GetConverter(property.PropertyType);
                var casted = foo.ConvertFromInvariantString(item.Value);

                property.SetValue(theClass, casted);
            }        

            return theClass;
        }
    }
}
