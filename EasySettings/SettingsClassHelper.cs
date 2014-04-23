namespace EasySettings
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    internal class SettingsClassHelper
    {
        #region constructors
        internal SettingsClassHelper()
            : this(new HttpContextWrapper(HttpContext.Current))
        {
        }

        internal SettingsClassHelper(HttpContext context)
            : this(new HttpContextWrapper(context))
        {
        }

        internal SettingsClassHelper(HttpContextBase httpContext)
        {
            var type = httpContext.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            var assembly = type == null ? null : type.Assembly;

            if (assembly == null) throw new Exception("Unable to determine executing assembly from HttpContext");


            TheClass = GetInstanceFromAssembly(assembly);
        }

        internal SettingsClassHelper(object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(BaseEasySettings)))
                throw new ArgumentException("Object does not implement base class", "obj");

            TheClass = obj;
        }

        internal SettingsClassHelper(Assembly assembly)
        {
            TheClass = GetInstanceFromAssembly(assembly);
        }
        #endregion

        internal object TheClass { get; set; }

        internal bool IsValidValue(string key, string value)
        {
            try
            {
                var property = TheClass.GetType().GetProperty(key);
                if (property == null || !property.CanWrite) return false;

                var typeConverter = TypeDescriptor.GetConverter(property.PropertyType);
                var casted = typeConverter.ConvertFromInvariantString(value);

                property.SetValue(TheClass, casted);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal PropertyInfo[] GetProperties()
        {
            return TheClass.GetType().GetProperties();
        }

        private static object GetInstanceFromAssembly(Assembly assembly)
        {
            return (from t in assembly.GetTypes()
                    where t.BaseType == (typeof(BaseEasySettings)) && t.GetConstructor(Type.EmptyTypes) != null
                    select (BaseEasySettings)Activator.CreateInstance(t)).Single();
        }

    }
}
