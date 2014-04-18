namespace EasySettings.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Setting storage using the HttpContext of the current web server.
    /// Suitable for short term storage. Any modified settings will be reset on application recycle
    /// </summary>
    public class HttpContextStorage : IStorage
    {
        private const string Prefix = "EasySetting";

        readonly HttpApplicationState _state;

        public HttpContextStorage() : this(HttpContext.Current.Application)
        {
        }

        public HttpContextStorage(HttpApplicationState state)
        {
            _state = state;
        }
        
        public void SaveSetting(string key, string value)
        {
            _state[Prefix + "-" + key] = value;
        }

        public string GetValue(string key)
        {
            return (_state[Prefix + "-" + key] ?? "").ToString();
        }

        public Dictionary<string, string> GetAllValues()
        {
            return _state.AllKeys.Where(item => item.StartsWith(Prefix)).ToDictionary(item => item.Replace(Prefix + "-", ""), item => _state[item].ToString());
        }

        public void Initialize()
        {
            // no initialization needed if this provider is used
        }
    }
}
