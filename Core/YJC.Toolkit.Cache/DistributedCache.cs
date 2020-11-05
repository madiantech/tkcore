using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public abstract class DistributedCache : ICache, IDisposable
    {
        protected DistributedCache(IDistributedCache distributedCache)
        {
            TkDebug.AssertArgumentNull(distributedCache, nameof(distributedCache), null);

            Cache = distributedCache;
        }

        public IDistributedCache Cache { get; }

        protected virtual bool GetCacheInstance(string key, ICacheDataConverter converter, out object value)
        {
            value = null;
            var byteData = Cache.Get(key);
            if (byteData == null)
                return false;

            DistributeData data = DistributeData.FromDistributeData(byteData);
            if (!data.Dependency.HasChanged)
            {
                value = data.CreateCacheObject(converter);
                return true;
            }
            else
                Cache.Remove(key);

            value = null;
            return false;
        }

        private void AddCacheInstance(string key, object instance, ICacheDataConverter converter,
            IDistributeCacheDependency dependency, bool useCache)
        {
            bool canCache = useCache || (BaseAppSetting.Current != null
                && BaseAppSetting.Current.UseCache);
            if (canCache)
            {
                DistributeData data = new DistributeData(converter, instance, dependency);
                var expiration = dependency.GetAbsoluteExpiration();
                DateTimeOffset absolute = expiration ?? new DateTimeOffset(DateTime.Today.AddDays(1));
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = absolute
                };
                SetCacheData(key, data, options);
            }
        }

        internal virtual void SetCacheData(string key, DistributeData data,
            DistributedCacheEntryOptions options)
        {
            Cache.Set(key, data.ToDistributeData(), options);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, string cacheName)
        {
            BaseCacheItemCreator creator = PlugInFactoryManager.CreateInstance<BaseCacheItemCreator>(
                CacheItemCreatorPlugInFactory.REG_NAME, cacheName);
            TkDebug.AssertNotNull(creator, $"{cacheName}不存在", this);
            ICacheDataConverter converter = GetDataConverter(creator);

            return GetCacheInstance(key, converter, out _);
        }

        public object GetItem(string key, BaseCacheItemCreator creator, params object[] args)
        {
            object result = null;

            ICacheDataConverter converter = GetDataConverter(creator);

            var distributeKey = GetDistributeKey(creator, key);
            if (GetCacheInstance(distributeKey, converter, out var cacheObject))
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
            ICacheDependency dependency = CacheUtil.SearchDependency(result);
            IDistributeCacheDependency distributeDependency = dependency as IDistributeCacheDependency;
            TkDebug.AssertNotNull(distributeDependency,
                $"附着的ICacheDependency（类型为{dependency.GetType()}）不支持IDistributeCacheDependency", dependency);
            AddCacheInstance(distributeKey, result, converter, distributeDependency,
                creator.Attribute.ForceCache);
            return result;
        }

        internal static ICacheDataConverter GetDataConverter(BaseCacheItemCreator creator)
        {
            ICacheDataConverter converter = creator as ICacheDataConverter;
            TkDebug.AssertNotNull(converter, $"{nameof(creator)}需要支持ICacheDataConverter接口", creator);
            return converter;
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        public virtual IEnumerable<string> GetUnusedKeys()
        {
            return null;
        }

        public virtual void TryRemoveList(IEnumerable<string> unusedKeys)
        {
        }

        public static string GetDistributeKey(BaseCacheItemCreator creator, string key)
        {
            TkDebug.AssertArgumentNull(creator, nameof(creator), null);
            TkDebug.AssertArgumentNullOrEmpty(key, nameof(key), null);

            string regName = creator.Attribute.GetRegName(creator.GetType());
            DistributeKey result = new DistributeKey(regName, key);
            return result.ToString();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}