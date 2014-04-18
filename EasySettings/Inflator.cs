namespace EasySettings
{
    using System;
    using System.ComponentModel;

    using Storage;

    public static class Inflator<T>
        where T : class
    {
        public static T InflateSettings(IStorage storage)
        {
            var theClass = Activator.CreateInstance<T>();
            var storedValues = storage.GetAllValues();

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

        public static T InflateSettings()
        {
            return InflateSettings(Configuration.PersistantSettingsProvider);
        }
    }
}
