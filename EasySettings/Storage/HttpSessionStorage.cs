namespace EasySettings.Storage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Setting storage using session storage.
    /// Suitable for short term, or user based settings. Settings will be lost when user loses session.
    /// </summary>
    public class HttpSessionStorage : IStorage
    {
        private const string Prefix = "EasySetting";

        HttpSessionStateBase _session;

        public HttpSessionStorage()
        {
        }

        public HttpSessionStorage(HttpSessionStateBase session)
        {
            _session = session;
        }

        protected HttpSessionStateBase Session
        {
            get
            {
                return _session ?? new HttpSessionStateWrapper(HttpContext.Current.Session);
            }
            set
            {
                _session = value;
            }
        }

        public void SaveSetting(string key, string value)
        {
            Session[Prefix + "-" + key] = value;
        }

        public string GetValue(string key)
        {
            return (Session[Prefix + "-" + key] ?? "").ToString();
        }

        public Dictionary<string, string> GetAllValues()
        {
            return Session.Keys.Cast<string>().Where(item => item.StartsWith(Prefix)).ToDictionary(item => item.Replace(Prefix + "-", ""), item => Session[item].ToString());
        }

        public void Initialize()
        {
            // no initialization needed if this provider is used
        }
    }
}
