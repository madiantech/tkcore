using System;
using System.Collections.Generic;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public static class CacheUtil
    {
        private const string DEFAULT_CACHE_KEY = "DefaultCache";
        private const string DEFAULT_DISTRIBUTED_CACHE_KEY = "DefaultDistributedCache";

        public static ICacheCreator DefaultCacheCreator { get; } = new SimpleCacheCreator();

        internal static ICacheDependency GetDependency(MemberInfo objType, object objInstance)
        {
            Attribute attribute = Attribute.GetCustomAttribute(
                objType, typeof(CacheDependencyAttribute));
            if (attribute == null)
            {
                ICacheDependencyCreator intf = objInstance as ICacheDependencyCreator;
                if (intf != null)
                    return intf.CreateCacheDependency();
            }
            else
                return attribute.Convert<CacheDependencyAttribute>().CreateObject();

            return null;
        }

        public static ICacheDependency SearchDependency(object data)
        {
            Type dataType = data.GetType();
            ICacheDependency dependency = CacheUtil.GetDependency(dataType, data);
            TkDebug.AssertNotNull(dependency, string.Format(ObjectUtil.SysCulture,
                "类型{0}没有附着CacheDependencyAttribute及其继承特性或者实现ICacheDependencyCreator接口，请检查",
                dataType), data);
            //TkDebug.AssertNotNull(dependency, string.Format(ObjectUtil.SysCulture,
            //    "类型{0}返回的CacheDependency是NULL，这是不允许的", attribute.GetType()), data);
            return dependency;
        }

        internal static bool IsCacheInstance(MemberInfo objType)
        {
            Attribute attribute = Attribute.GetCustomAttribute(
                objType, typeof(CacheInstanceAttribute));
            return attribute != null;
        }

        public static string GetSimpleDefaultValue(string keyName)
        {
            TkDebug.AssertArgumentNullOrEmpty(keyName, nameof(keyName), null);

            TkDebug.ThrowIfNoGlobalVariable();

            return BaseGlobalVariable.Current.DefaultValue.GetSimpleDefaultValue(keyName);
        }

        public static string GetSimpleDefaultValue(string keyName, string defaultValue)
        {
            var value = GetSimpleDefaultValue(keyName);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        internal static ICacheCreator GetDefaultCacheCreator()
        {
            string value = GetSimpleDefaultValue(DEFAULT_CACHE_KEY);
            if (string.IsNullOrEmpty(value))
                return DefaultCacheCreator;

            return CreateCache(value);
        }

        private static ICacheCreator CreateCache(string name)
        {
            ICacheCreator creator = PlugInFactoryManager.CreateInstance<ICacheCreator>(
                CacheCreatorPlugInFactory.REG_NAME, name);

            return creator;
        }

        internal static ICacheCreator GetDefaultDistributeCacheCreator()
        {
            string value = GetSimpleDefaultValue(DEFAULT_DISTRIBUTED_CACHE_KEY);
            if (string.IsNullOrEmpty(value))
                return DefaultCacheCreator;

            return CreateCache(value);
        }

        internal static void TryCleanCache(IEnumerable<ICache> caches)
        {
            if (caches == null)
                return;

            List<(ICache Cache, IEnumerable<string> Keys)> list = new List<(ICache Cache, IEnumerable<string> Keys)>();
            foreach (var cache in caches)
            {
                if (cache == null)
                    continue;

                var unusedKeys = cache.GetUnusedKeys();
                if (unusedKeys != null)
                {
                    list.Add((cache, unusedKeys));
                }
            }

            foreach (var item in list)
            {
                var keys = item.Keys;
                item.Cache.TryRemoveList(keys);
            }
        }
    }
}