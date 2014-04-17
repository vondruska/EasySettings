using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheTest.App_Start
{
    using EasySettings.Storage;

    public class EasySettingsConfig
    {
        public static void PreStart()
        {
            EasySettings.Configuration.PersistantSettingsProvider = new HttpContextStorage();
        }
    }
}