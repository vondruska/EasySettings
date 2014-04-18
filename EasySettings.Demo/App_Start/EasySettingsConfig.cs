[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(EasySettings.Demo.App_Start.EasySettingsConfig), "PreStart")]


namespace EasySettings.Demo.App_Start
{
    public class EasySettingsConfig
    {
        public static void PreStart()
        {
            Configuration.PersistantSettingsProvider = new Storage.HttpContextStorage();
        }
    }
}