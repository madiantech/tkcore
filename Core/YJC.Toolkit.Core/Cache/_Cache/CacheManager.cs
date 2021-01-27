using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.Cache
{
    public sealed class CacheManager : IDisposable
    {
        private readonly Dictionary<string, ICache> fCaches;
        private readonly ReaderWriterLockSlim fCacheLock;
        private ICacheCreator fDefaultCreator;
        private ICacheCreator fDefaultDistributedCreator;

        internal CacheManager()
        {
            fCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            fCaches = new Dictionary<string, ICache>();
        }

        #region IDisposable 成员

        public void Dispose()
        {
            CacheLock.Dispose();
            foreach (var item in fCaches.Values)
                item.DisposeObject();
            fCaches.Clear();
        }

        #endregion IDisposable 成员

        public ReaderWriterLockSlim CacheLock
        {
            get
            {
                return fCacheLock;
            }
        }

        public ICacheCreator DefaultCreator
        {
            get
            {
                if (fDefaultCreator == null)
                    fDefaultCreator = CacheUtil.GetDefaultCacheCreator();
                return fDefaultCreator;
            }
        }

        public ICacheCreator DefaultDistributedCreator
        {
            get
            {
                if (fDefaultDistributedCreator == null)
                    fDefaultDistributedCreator = CacheUtil.GetDefaultDistributeCacheCreator();
                return fDefaultDistributedCreator;
            }
        }

        private ICache GetCache(string cacheName)
        {
            ICache cache;
            cache = CacheLock.ReadLockAction(() => ObjectUtil.TryGetValue(fCaches, cacheName));
            return cache;
        }

        public bool Contains(string cacheName, string key)
        {
            TkDebug.AssertArgumentNullOrEmpty(cacheName, "cacheName", this);
            TkDebug.AssertArgumentNullOrEmpty(key, "key", this);

            ICache cache = GetCache(cacheName);
            return cache == null ? false : cache.ContainsKey(key, cacheName);
        }

        public object GetCacheItem(string cacheName, string key,
            params object[] args)
        {
            TkDebug.AssertArgumentNullOrEmpty(cacheName, "cacheName", this);
            TkDebug.AssertArgumentNullOrEmpty(key, "key", this);

            ICache cache = GetCache(cacheName);
            BaseCacheItemCreator creator = PlugInFactoryManager.CreateInstance<BaseCacheItemCreator>(
                CacheItemCreatorPlugInFactory.REG_NAME, cacheName);
            TkDebug.AssertNotNull(creator, $"{cacheName}不存在", this);

            if (cache == null)
            {
                ICacheCreator cacheCreator = creator.CacheCreator;
                if (cacheCreator == null)
                    cacheCreator = creator.SupportDistributed ? DefaultDistributedCreator : DefaultCreator;
                cache = cacheCreator.CreateCache(cacheName);
                CacheLock.WriteLockAction(() =>
                {
                    if (!fCaches.ContainsKey(cacheName))
                    {
                        fCaches.Add(cacheName, cache);
                        return true;
                    }
                    return false;
                });
            }
            return cache.GetItem(creator.TransformCacheKey(key), creator, args);
        }

        public void Remove(string cacheName, string key)
        {
            TkDebug.AssertArgumentNullOrEmpty(cacheName, "cacheName", this);
            TkDebug.AssertArgumentNullOrEmpty(key, "key", this);

            ICache cache = GetCache(cacheName);
            if (cache != null)
                cache.Remove(key);
        }

        public void Clear(string cacheName)
        {
            TkDebug.AssertArgumentNullOrEmpty(cacheName, nameof(cacheName), this);

            ICache cache = GetCache(cacheName);
            if (cache != null)
                cache.Clear();
        }

        public void Clean(string cacheName)
        {
            TkDebug.AssertArgumentNullOrEmpty(cacheName, nameof(cacheName), this);

            ICache cache = GetCache(cacheName);
            if (cache != null)
                CacheUtil.TryCleanCache(EnumUtil.Convert(cache));
        }

        public void Clean()
        {
            CacheUtil.TryCleanCache(fCaches.Values.ToArray());
        }

        //public static void SetDefaultCacheCreator(ICacheCreator creator)
        //{
        //    TkDebug.AssertArgumentNull(creator, "creator", null);

        //    TkDebug.ThrowIfNoGlobalVariable();
        //    BaseGlobalVariable.Current.CacheManager.fDefaultCreator = creator;
        //}

        public static object GetItem(string cacheName, string key, params object[] args)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            return BaseGlobalVariable.Current.CacheManager.GetCacheItem(cacheName, key, args);
        }

        public static bool ContainsKey(string cacheName, string key)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            return BaseGlobalVariable.Current.CacheManager
                .Contains(cacheName, key);
        }

        public static void RemoveKey(string cacheName, string key)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            BaseGlobalVariable.Current.CacheManager.Remove(cacheName, key);
        }

        public static void ClearCache(string cacheName)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            BaseGlobalVariable.Current.CacheManager.Clear(cacheName);
        }

        public static void CleanCache(string cacheName)
        {
            TkDebug.ThrowIfNoGlobalVariable();
            BaseGlobalVariable.Current.CacheManager.Clean(cacheName);
        }

        public static void CleanCache()
        {
            TkDebug.ThrowIfNoGlobalVariable();
            BaseGlobalVariable.Current.CacheManager.Clean();
        }

        public override string ToString()
        {
            return string.Format(ObjectUtil.SysCulture,
                "缓存对象管理器，已经有{0}种类型的缓存", fCaches.Count);
        }
    }
}