using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Cache
{
    public abstract class BaseCacheItemCreator
    {
        protected BaseCacheItemCreator()
            : this(0)
        {
        }

        protected BaseCacheItemCreator(int capacity)
        {
            TkDebug.AssertArgument(capacity >= 0, "capacity", string.Format(ObjectUtil.SysCulture,
                "参数capacity必须不小于0，现在值为{0}", capacity), null);

            //CacheCreator = new SimpleCacheCreator(capacity);
            Initialize();
        }

        protected BaseCacheItemCreator(ICacheCreator cacheCreator)
        {
            TkDebug.AssertArgumentNull(cacheCreator, "cacheCreator", null);

            CacheCreator = cacheCreator;
            Initialize();
        }

        public ICacheCreator CacheCreator { get; private set; }

        public virtual bool SupportDistributed { get => false; }

        public CacheItemCreatorAttribute Attribute { get; private set; }

        private void Initialize()
        {
            Type type = GetType();
            var attr = global::System.Attribute.GetCustomAttribute(type,
                typeof(CacheItemCreatorAttribute));
            TkDebug.AssertNotNull(attr, string.Format(ObjectUtil.SysCulture,
                "类型{0}没有附着CacheItemCreatorAttribute", type), null);
            Attribute = attr.Convert<CacheItemCreatorAttribute>();
        }

        public abstract object Create(string key, params object[] args);

        public virtual string TransformCacheKey(string key)
        {
            return key;
        }

        public static BaseCacheItemCreator Create(string creatorName)
        {
            TkDebug.AssertArgumentNullOrEmpty(creatorName, "creatorName", null);

            BaseCacheItemCreator result = PlugInFactoryManager.CreateInstance<BaseCacheItemCreator>(
                CacheItemCreatorPlugInFactory.REG_NAME, creatorName);
            return result;
        }
    }
}