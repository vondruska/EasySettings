[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(EasySettings.Demo.App_Start.EasySettingsConfig), "PreStart")]


namespace EasySettings.Demo.App_Start
{
    using SettingsStorage;

    public class EasySettingsConfig
    {
        public static void PreStart()
        {
            Configuration.SettingsProvider = new HttpContextSettingsStorage();
        }
    }
}