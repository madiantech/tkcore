namespace YJC.Toolkit.Cache
{
    public interface ICacheCreator
    {
        ICache CreateCache(string cacheName);
    }
}