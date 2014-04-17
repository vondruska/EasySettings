using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySettings.Cache
{
    class NoCache : ICache
    {

        public void Store(object obj)
        {
            return;
        }

        public void Clear()
        {
            return;
        }

        public object GetObject()
        {
            return null;
        }
    }
}
