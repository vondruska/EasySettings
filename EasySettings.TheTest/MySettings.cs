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
    }

    public enum TestEnum
    {
        ThisIs0,
        ThisIs1
    }
}