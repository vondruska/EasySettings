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

        HttpApplicationStateBase _state;

        public HttpContextStorage()
        {
        }

        public HttpContextStorage(HttpApplicationStateBase state)
        {
            _state = state;
        }

        protected HttpApplicationStateBase State
        {
            get
            {
                return _state ?? new HttpApplicationStateWrapper(HttpContext.Current.Application);
            }
            set
            {
                _state = value;
            }
        }
        
        public void SaveSetting(string key, string value)
        {
            State[Prefix + "-" + key] = value;
        }

        public string GetValue(string key)
        {
            return (State[Prefix + "-" + key] ?? "").ToString();
        }

        public Dictionary<string, string> GetAllValues()
        {
            return State.AllKeys.Where(item => item.StartsWith(Prefix)).ToDictionary(item => item.Replace(Prefix + "-", ""), item => State[item].ToString());
        }

        public void Initialize()
        {
            // no initialization needed if this provider is used
        }
    }
}
