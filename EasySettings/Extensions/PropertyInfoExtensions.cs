namespace EasySettings.Extensions
{
    using System.ComponentModel;
    using System.Reflection;

    public static class PropertyInfoExtensions
    {
        public static string GetDescription(this PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? "" : attribute.Description;
        }
    }
}
