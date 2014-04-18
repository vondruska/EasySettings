namespace EasySettings.TheTest
{
    using System.ComponentModel;

    public class MySettings : BaseEasySettings
    {
        public MySettings()
        {
            MiniProfiler = true;
        }

        [Description("Allow MiniProfiler to run")]
        public bool MiniProfiler { get; set; }

        public bool MyTest { get; set; }

        public TestEnum MyTestEnum { get; set; }

        [Description("This is an integer value with a really long description that should wrap eventually")]
        public int IntegerValue { get; set; }
    }

    public enum TestEnum
    {
        ThisIs0,
        ThisIs1
    }
}