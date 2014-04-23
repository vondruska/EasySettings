namespace EasySettings
{
    using System;
    using System.ComponentModel;

    internal class Binder
    {
        internal T Bind<T>(T value, ISettingsStorage settingsStorage)
        {
            var storedValues = settingsStorage.GetAllValues();

            foreach (var item in storedValues)
            {
                var property = value.GetType().GetProperty(item.Key);
                if (property == null || !property.CanWrite) continue;

                var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                var casted = typeConverter.ConvertFromInvariantString(item.Value);

                property.SetValue(value, casted);
            }

            return value;
        }

        internal T Bind<T>()
        {
            var theClass = Activator.CreateInstance<T>();

            return Bind<T>(theClass, Configuration.SettingsProvider);
        }

        internal T Bind<T>(T value)
        {
            return Bind<T>(value, Configuration.SettingsProvider);
        }
    }
}
