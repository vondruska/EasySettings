namespace EasySettings
{
    public static class Current<T> 
        where T : class, new()
    {
        public static T Settings
        {
            get { return Inflator.InflateSettings<T>(); }
        }
    }
}
