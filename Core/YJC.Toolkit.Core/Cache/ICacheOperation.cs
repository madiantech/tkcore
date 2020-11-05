namespace YJC.Toolkit.Cache
{
    internal interface ICacheOperation
    {
        object GetCacheInstance(string key);

        void AddCacheInstance(string key, object instance,
            ICacheDependency dependency, bool useCache);
    }
}
