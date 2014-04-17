namespace EasySettings.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Setting storage using the HttpContext of the current web server.
    /// Sutiable for short term storage. Any modified settings will be reset on application recycle
    /// </summary>
    public class HttpContextStorage : IStorage
    {
        private const string Prefix = "EasySetting";

        public void SaveSetting(string key, string value)
        {
            HttpContext.Current.Application[Prefix + "-" + key] = value;
        }

        public object GetValue(string key)
        {
            return HttpContext.Current.Application[Prefix + "-" + key];
        }

        public Dictionary<string, string> GetAllValues()
        {
            return HttpContext.Current.Application.AllKeys.Where(item => item.StartsWith(Prefix)).ToDictionary(item => item.Replace(Prefix + "-", ""), item => HttpContext.Current.Application[item].ToString());
        }

        public void Initialize()
        {
            // no initialization needed if this provider is used
        }
    }
}
