namespace EasySettings.Cache
{
    public interface ICache
    {
        void Store(object obj);

        void Clear();

        object GetObject();
    }
}
