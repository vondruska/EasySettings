using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySettings.Storage
{
    using System.Collections.Concurrent;

    public class LocalStorage : IStorage
    {
        private Dictionary<string, string> _storage = new Dictionary<string, string>();
        public void SaveSetting(string key, string value)
        {
            _storage[key] = value;
        }

        public object GetValue(string key)
        {
            return _storage[key];
        }

        public Dictionary<string, string> GetAllValues()
        {
            return _storage;
        }

        public void Initialize()
        {
            return;
        }
    }
}
