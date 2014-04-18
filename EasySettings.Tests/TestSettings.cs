namespace EasySettings.Tests
{
    using System.ComponentModel;

    enum MySettingsEnum
    {
        DefaultValue = 0,
        FirstValue = 1,
        SecondValue = 2
    }
    
    class TestSettings : BaseEasySettings
    {

        public TestSettings()
        {
            BooleanStartsTrue = true;
        }

        [Description("My boolean setting")]
        public bool BooleanValue { get; set; }

        [Description("My integer setting")]
        public int IntegerValue { get; set; }
        
        [Description("My string setting")]
        public string StringValue { get; set; }
        
        [Description("My enum setting")]
        public MySettingsEnum EnumValue { get; set; }

        public bool BooleanStartsTrue { get; set; }
    }
}
