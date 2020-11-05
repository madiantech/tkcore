using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.Cache
{
    internal sealed class SimpleCache : ICache, IDisposable, ICacheOperation
    {
        private Dictionary<string, CacheData> fCacheTable;
        private readonly ReaderWriterLockSlim fLock;

        public SimpleCache()
            : this(0)
        {
        }

        public SimpleCache(int capacity)
        {
            fCacheTable = new Dictionary<string, CacheData>(capacity);
            TkDebug.ThrowIfNoGlobalVariable();
            fLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        #region ICacheOperation 成员

        public object GetCacheInstance(string key)
        {
            object result;
            GetCacheInstance(key, out result);
            return result;
        }

        public void AddCacheInstance(string key, object instance,
            ICacheDependency dependency, bool useCache)
        {
            bool canCache = useCache || (BaseAppSetting.Current != null
                && BaseAppSetting.Current.UseCache);
            if (canCache)
            {
                CacheData data = new CacheData(instance, dependency);
                if (dependency != NoDependency.Dependency)
                {
                    fLock.WriteLockAction(() =>
                    {
                        if (!fCacheTable.ContainsKey(key))
                        {
                            fCacheTable.Add(key, data);
                            return true;
                        }
                        return false;
                    });
                }
            }
        }

        #endregion ICacheOperation 成员

        #region ICache 成员

        public bool ContainsKey(string key, string cacheName)
        {
            return GetCacheInstance(key, out _);
        }

        public object GetItem(string key, BaseCacheItemCreator creator, params object[] args)
        {
            object result = null;

            object cacheObject;
            if (GetCacheInstance(key, out cacheObject))
            {
                return cacheObject;
            }
            try
            {
                result = creator.Create(key, args);
                TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                    "类型为{0}缓存对象创建者没有创建缓存对象，请确认",
                    creator.GetType()), creator);
            }
            catch (Exception ex)
            {
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型为{0}缓存对象创建者在创建对象时发生例外，请检查原因",
                    creator.GetType()), ex, creator);
            }
            AddCacheInstance(key, result, CacheUtil.SearchDependency(result),
                creator.Attribute.ForceCache);
            return result;
        }

        public void Remove(string key)
        {
            CacheData data = fLock.ReadLockAction(() =>
            {
                return ObjectUtil.TryGetValue(fCacheTable, key);
            });
            if (data != null)
            {
                fLock.WriteLockAction(() =>
                {
                    return fCacheTable.Remove(key);
                });
                data.Dispose();
            }
        }

        public void Clear()
        {
            fLock.WriteLockAction(() =>
            {
                foreach (var item in fCacheTable)
                    item.Value.Dispose();
                fCacheTable.Clear();
                return true;
            });
        }

        public IEnumerable<string> GetUnusedKeys()
        {
            var result = fLock.TryReadLockAction(() =>
            {
                List<string> list = new List<string>();
                foreach (var item in fCacheTable)
                    if (item.Value.Dependency.HasChanged)
                        list.Add(item.Key);

                return list;
            });

            if (result.Value?.Count == 0)
                return null;

            return result.Value;
        }

        public void TryRemoveList(IEnumerable<string> unusedKeys)
        {
            if (unusedKeys == null)
                return;

            foreach (var key in unusedKeys)
            {
                CacheData data = null;
                if (fLock.TryEnterUpgradeableReadLock(RWLockExtension.TIME_OUT))
                {
                    try
                    {
                        data = ObjectUtil.TryGetValue(fCacheTable, key);
                        if (data != null)
                        {
                            if (fLock.TryEnterWriteLock(RWLockExtension.TIME_OUT))
                                try
                                {
                                    fCacheTable.Remove(key);
                                }
                                finally
                                {
                                    fLock.ExitWriteLock();
                                }
                        }
                    }
                    finally
                    {
                        fLock.ExitUpgradeableReadLock();
                    }
                }
                data?.Dispose();
            }
        }

        #endregion ICache 成员

        #region IDisposable 成员

        public void Dispose()
        {
            foreach (CacheData item in fCacheTable.Values)
                item.Dispose();
            fCacheTable.Clear();
            fCacheTable = null;
            fLock.Dispose();
        }

        #endregion IDisposable 成员

        private bool GetCacheInstance(string key, out object value)
        {
            CacheData data = fLock.ReadLockAction(() =>
            {
                return ObjectUtil.TryGetValue(fCacheTable, key);
            });
            if (data != null)
            {
                if (!data.Dependency.HasChanged)
                {
                    value = data.Data;
                    return true;
                }
                else
                {
                    fLock.WriteLockAction(() =>
                    {
                        return fCacheTable.Remove(key);
                    });
                    data.Dispose();
                }
            }
            value = null;
            return false;
        }
    }
}