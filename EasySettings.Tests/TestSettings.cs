namespace EasySettings.Tests
{
    class TestSettings : BaseEasySettings
    {
        public TestSettings()
        {
            BooleanStartsTrue = true;
        }

        [System.ComponentModel.Description("My boolean setting")]
        public bool BooleanValue { get; set; }

        [System.ComponentModel.Description("My integer setting")]
        public int IntegerValue { get; set; }
        
        [System.ComponentModel.Description("My string setting")]
        public string StringValue { get; set; }
        
        [System.ComponentModel.Description("My enum setting")]
        public MySettingsEnum EnumValue { get; set; }

        public bool BooleanStartsTrue { get; set; }
    }

    enum MySettingsEnum
    {
        DefaultValue = 0,
        FirstValue = 1,
        SecondValue = 2
    }
}
