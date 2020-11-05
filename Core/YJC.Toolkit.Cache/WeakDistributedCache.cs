using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Threading;

namespace YJC.Toolkit.Cache
{
    public abstract class WeakDistributedCache : DistributedCache
    {
        private readonly ReaderWriterLockSlim fLock;
        private readonly ConcurrentDictionary<string, WeakReference<DistributeData>> fCacheTable;

        protected WeakDistributedCache(IDistributedCache distributedCache)
            : base(distributedCache)
        {
            fCacheTable = new ConcurrentDictionary<string, WeakReference<DistributeData>>();
            TkDebug.ThrowIfNoGlobalVariable();
            fLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                fLock.Dispose();

            base.Dispose(disposing);
        }

        protected override bool GetCacheInstance(string key, ICacheDataConverter converter, out object value)
        {
            value = null;
            DistributeData data;
            if (fCacheTable.TryGetValue(key, out var localCache))
            {
                if (localCache.TryGetTarget(out data))
                {
                    if (!data.Dependency.HasChanged)
                    {
                        value = data.CreateCacheObject(converter);
                        return true;
                    }
                    else
                    {
                        Remove(key);
                        return false;
                    }
                }
            }

            var byteData = Cache.Get(key);
            if (byteData == null)
                return false;

            data = DistributeData.FromDistributeData(byteData);
            if (!data.Dependency.HasChanged)
            {
                value = data.CreateCacheObject(converter);
                fCacheTable[key] = new WeakReference<DistributeData>(data);
                return true;
            }
            else
                Remove(key);

            value = null;
            return false;
        }

        internal override void SetCacheData(string key, DistributeData data, DistributedCacheEntryOptions options)
        {
            fLock.WriteLockAction(() =>
            {
                fCacheTable.TryAdd(key, new WeakReference<DistributeData>(data));
                Cache.Set(key, data.ToDistributeData(), options);
                return true;
            });
        }

        public override void Remove(string key)
        {
            fLock.WriteLockAction(() =>
            {
                RemoveKey(key);
                return true;
            });
        }

        private void RemoveKey(string key)
        {
            fCacheTable.TryRemove(key, out _);
            Cache.RemoveAsync(key);
        }

        public override IEnumerable<string> GetUnusedKeys()
        {
            var result = fLock.TryReadLockAction(() =>
            {
                List<string> list = new List<string>();
                foreach (var item in fCacheTable)
                {
                    if (item.Value.TryGetTarget(out var data))
                    {
                        if (data.Dependency.HasChanged)
                            list.Add(item.Key);
                    }
                }
                return list;
            });

            if (result.Value?.Count == 0)
                return null;
            return result.Value;
        }

        public override void TryRemoveList(IEnumerable<string> unusedKeys)
        {
            if (unusedKeys == null)
                return;
            foreach (var key in unusedKeys)
                fLock.TryWriteLockAction(() =>
                {
                    RemoveKey(key);
                    return true;
                });
        }
    }
}