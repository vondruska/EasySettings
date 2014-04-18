using System;
using System.Linq;

namespace EasySettings
{
    using System.ComponentModel;
    using System.Reflection;
    using System.Web;

    public class SettingsClassHelper
    {
        public object TheClass { get; set; }

        public SettingsClassHelper()
            : this(new HttpContextWrapper(HttpContext.Current))
        {
        }

        public SettingsClassHelper(HttpContext context)
            : this(new HttpContextWrapper(context))
        {
        }

        public SettingsClassHelper(HttpContextBase httpContext)
        {
            var type = httpContext.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            var assembly = type == null ? null : type.Assembly;

            if (assembly == null) throw new Exception("Unable to determine executing assembly from HttpContext");


            TheClass = (from t in assembly.GetTypes()
                    where t.BaseType == (typeof(BaseEasySettings)) && t.GetConstructor(Type.EmptyTypes) != null
                    select (BaseEasySettings)Activator.CreateInstance(t)).Single();
        }

        public SettingsClassHelper(object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(BaseEasySettings)))
                throw new ArgumentException("Object does not implement base class", "obj");

            TheClass = obj;
        }

        public bool IsValidValue(string key, string value)
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

        public PropertyInfo[] GetProperties()
        {
            return TheClass.GetType().GetProperties();
        }

    }
}
