namespace EasySettings.Extensions
{
    using System.ComponentModel;
    using System.Reflection;

    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Get the description attribute of a property
        /// </summary>
        /// <param name="propertyInfo">The property you want a description of</param>
        /// <returns>Description attribute value, empty string if not defined</returns>
        public static string GetDescription(this PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            return attribute == null ? "" : attribute.Description;
        }
    }
}
