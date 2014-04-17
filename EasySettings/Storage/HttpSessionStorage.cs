namespace EasySettings.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Setting storage using session storage.
    /// Suitable for short term, or user based settings. Settings will be lost when user loses session
    /// </summary>
    class HttpSessionStorage
    {
        private const string Prefix = "EasySetting";

        public void SaveSetting(string key, object value)
        {
            HttpContext.Current.Session[Prefix + "-" + key] = value;
        }

        public object GetValue(string key)
        {
            return HttpContext.Current.Session[Prefix + "-" + key];
        }

        public Dictionary<string, string> GetAllValues()
        {
            return HttpContext.Current.Session.Keys.Cast<string>().Where(item => item.StartsWith(Prefix)).ToDictionary(item => item.Replace(Prefix + "-", ""), item => HttpContext.Current.Session[item].ToString());
        }

        public void Initialize()
        {
            // no initialization needed if this provider is used
        }
    }
}
