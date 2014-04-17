namespace EasySettings.Web
{
    using System.Collections.Generic;

    internal class ViewModel
    {
        public string Token { get; set; }
        public IEnumerable<SettingViewModel> Settings { get; set; }
    }

    internal class SettingViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
    }
}
