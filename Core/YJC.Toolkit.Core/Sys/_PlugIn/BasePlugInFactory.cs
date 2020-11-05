using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    public abstract class BasePlugInFactory
    {
        private readonly Dictionary<string, CodeRegItem> fCodePlugIns;
        private ICacheOperation fCache;

        protected BasePlugInFactory(string name, string description)
        {
            TkDebug.AssertArgumentNullOrEmpty(name, "name", null);

            Name = name;
            Description = description;
            fCodePlugIns = new Dictionary<string, CodeRegItem>();
        }

        private ICacheOperation Cache
        {
            get
            {
                if (fCache == null)
                {
                    //switch (CacheType)
                    //{
                    //    case CacheType.Simple:
                    //        fCache = new SimpleCache(CacheCapacity);
                    //        break;
                    //    case CacheType.LRU:
                    //        fCache = new SimpleLRUCache(CacheCapacity,
                    //            SimpleLRUCacheCreator.DEFAULT_MIN_AGE,
                    //            SimpleLRUCacheCreator.DEFAULT_MAX_AGE);
                    //        break;
                    //    default:
                    //        TkDebug.ThrowImpossibleCode(this);
                    //        break;
                    //}
                    fCache = new SimpleCache(CacheCapacity);
                }
                return fCache;
            }
        }

        internal event EventHandler<RegItemEventArgs> FailGetRegItem;

        public string Name { get; private set; }

        public string Description { get; private set; }

        public int CacheCapacity { get; set; }

        public virtual int Count
        {
            get
            {
                return fCodePlugIns.Count;
            }
        }

        private void CachePlugInObject(string regName, object instance)
        {
            Type objType = instance.GetType();
            bool isCache = CacheUtil.IsCacheInstance(objType);
            if (!isCache)
                return;
            ICacheDependency dependency = CacheUtil.GetDependency(objType, instance);
            if (dependency == null)
                dependency = AlwaysDependency.Dependency;

            Cache.AddCacheInstance(regName, instance, dependency, false);
        }

        private T OnInstanceCreated<T>(string regName, T result) where T : class
        {
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "无法创建{0}中注册名为{1}的插件", Name, regName), this);
            OnPlugInCreated(regName, result);
            CachePlugInObject(regName, result);
            return result;
        }

        internal virtual void OnFailGetRegItem(RegItemEventArgs ea)
        {
            EventHandler<RegItemEventArgs> handler = FailGetRegItem;
            if (handler != null)
                handler(this, ea);
        }

        internal void InternalAddPlugIn(string regName, CodeRegItem regItem)
        {
            fCodePlugIns.Add(regName, regItem);
        }

        protected virtual bool Add(string regName, BasePlugInAttribute attribute, Type type)
        {
            lock (this)
            {
                if (!Contains(regName))
                {
                    CodeRegItem regItem = new CodeRegItem(regName, attribute, type);
                    InternalAddPlugIn(regName, regItem);
                    return true;
                }
                return false;
            }
        }

        public bool Add(BasePlugInAttribute attribute, Type type)
        {
            TkDebug.AssertArgumentNull(attribute, "attribute", this);
            TkDebug.AssertArgumentNull(type, "type", this);

            TkDebug.Assert(attribute.FactoryName == Name, string.Format(ObjectUtil.SysCulture,
                "调用错误，Attribute的工厂名{0}和当前工厂的不一致", attribute.FactoryName), this);

            string regName = attribute.GetRegName(type);
            return Add(regName, attribute, type);
        }

        public bool Add(Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", this);

            var attrs = Attribute.GetCustomAttributes(type, typeof(BasePlugInAttribute), false);
            var attr = (from item in attrs
                        let plugAttr = item as BasePlugInAttribute
                        where plugAttr.FactoryName == Name
                        select plugAttr).FirstOrDefault();
            if (attr == null)
                TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                    "类型{0}没有标注工厂对应的Attribute，请检查", type), this);
            return Add(attr, type);
        }

        public virtual bool Contains(string regName)
        {
            if (string.IsNullOrEmpty(regName))
                return false;

            return fCodePlugIns.ContainsKey(regName);
        }

        internal virtual BaseRegItem GetRegItem(string regName)
        {
            BaseRegItem result = ObjectUtil.TryGetValue(fCodePlugIns, regName);

            if (result == null)
            {
                RegItemEventArgs eventArgs = new RegItemEventArgs(regName);
                OnFailGetRegItem(eventArgs);
                result = eventArgs.RegItem;
            }

            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "在{1}的插件工厂中没有找到注册名为{0}的插件", regName, Name), this);
            return result;
        }

        protected virtual T InternalCreateInstance<T>(string regName) where T : class
        {
            BaseRegItem item = GetRegItem(regName);
            return item.CreateInstance<T>();
        }

        protected virtual T InternalCreateInstance<T>(string regName, params object[] args) where T : class
        {
            BaseRegItem item = GetRegItem(regName);
            return item.CreateInstance<T>(args);
        }

        protected virtual void OnPlugInCreated<T>(string regName, T obj)
        {
        }

        protected virtual void SearchXmlPath(string basePath)
        {
        }

        internal void SearchXmlPlugIn(string basePath)
        {
            try
            {
                SearchXmlPath(basePath);
            }
            catch (Exception ex)
            {
                BaseGlobalVariable.Current.HandleStartedExeception("SXmlPlug", GetType(), ex);
            }
        }

        public bool DestroyCacheInstance(string regName)
        {
            if (string.IsNullOrEmpty(regName))
                return false;

            if (fCache == null)
                return false;

            SimpleCache cache = (SimpleCache)Cache;
            if (cache.ContainsKey(regName, null))
            {
                cache.Remove(regName);
                return true;
            }

            return false;
        }

        public T CreateInstance<T>(string regName) where T : class
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            object cacheInstance = Cache.GetCacheInstance(regName);
            if (cacheInstance != null)
                return cacheInstance.Convert<T>();
            T result = InternalCreateInstance<T>(regName);

            return OnInstanceCreated<T>(regName, result);
        }

        public T CreateInstance<T>(string regName, params object[] args) where T : class
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);
            TkDebug.AssertArgumentNull(args, "args", this);

            object cacheInstance = Cache.GetCacheInstance(regName);
            if (cacheInstance != null)
                return cacheInstance.Convert<T>();
            T result = InternalCreateInstance<T>(regName, args);

            return OnInstanceCreated<T>(regName, result);
        }

        public void EnumableCodePlugIn(Action<string, Type, BasePlugInAttribute> action)
        {
            if (action == null)
                return;

            foreach (var item in fCodePlugIns)
                action(item.Key, item.Value.RegType, item.Value.Attribute);
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Description))
                return Description + "的插件工厂";
            else if (!string.IsNullOrEmpty(Name))
                return Name + "的插件工厂";
            else
                return base.ToString();
        }
    }
}