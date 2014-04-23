namespace EasySettings.SettingsStorage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Setting settingsStorage using the HttpContext of the current web server.
    /// Suitable for short term settingsStorage. Any modified settings will be reset on application recycle
    /// </summary>
    public class HttpContextSettingsStorage : ISettingsStorage
    {
        private const string Prefix = "EasySetting";

        HttpApplicationStateBase _state;

        public HttpContextSettingsStorage()
        {
        }

        public HttpContextSettingsStorage(HttpApplicationStateBase state)
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
