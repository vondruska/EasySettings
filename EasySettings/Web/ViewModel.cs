namespace EasySettings.Web
{
    using System.Collections.Generic;

    internal class ViewModel
    {
        public string Token { get; set; }
        public IEnumerable<SettingViewModel> Settings { get; set; }
    }

    public class SettingViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public SettingType Type { get; set; }
        public string[] PossibleValues { get; set; }
    }

    public enum SettingType
    {
        String = 0,
        Boolean = 1,
        Enum = 2
    }
}
