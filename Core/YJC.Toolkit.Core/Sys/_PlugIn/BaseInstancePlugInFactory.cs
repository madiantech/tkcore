using System;
using System.Collections.Generic;
using System.Reflection;
using YJC.Toolkit.Cache;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseInstancePlugInFactory : BasePlugInFactory
    {
        private readonly Dictionary<string, InstanceRegItem> fInstancePlugIns;

        protected BaseInstancePlugInFactory(string name, string description)
            : base(name, description)
        {
            fInstancePlugIns = new Dictionary<string, InstanceRegItem>();
        }

        public override int Count
        {
            get
            {
                return fInstancePlugIns.Count + base.Count;
            }
        }

        internal InstanceRegItem AddInstance(string regName, BasePlugInAttribute attribute,
            object instance, ICacheDependency dependency)
        {
            lock (this)
            {
                if (!Contains(regName))
                {
                    InstanceRegItem instanceRegItem = new InstanceRegItem(regName,
                        attribute, instance, dependency);
                    fInstancePlugIns.Add(regName, instanceRegItem);
                    return instanceRegItem;
                }
                return null;
            }
        }

        protected override bool Add(string regName, BasePlugInAttribute attribute, Type type)
        {
            Attribute instanceAttr = Attribute.GetCustomAttribute(type, typeof(InstancePlugInAttribute));
            if (instanceAttr != null)
            {
                BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
                FieldInfo fieldInfo = type.GetField("Instance", flags);
                if (fieldInfo == null)
                {
                    fieldInfo = type.GetField("INSTANCE", flags);
                    if (fieldInfo == null)
                        return false;
                }
                object value = ObjectUtil.GetStaticValue(fieldInfo);
                if (value == null)
                    return false;
                ICacheDependency dependency = CacheUtil.GetDependency(type, null);
                if (dependency == null)
                    dependency = AlwaysDependency.Dependency;

                return AddInstance(regName, attribute, value, dependency) != null;
            }
            else
                return base.Add(regName, attribute, type);
        }

        public void EnumableInstancePlugIn(Action<string, object, BasePlugInAttribute> action)
        {
            if (action == null)
                return;

            foreach (var item in fInstancePlugIns)
                action(item.Key, item.Value.Instance, item.Value.Attribute);
        }

        public override bool Contains(string regName)
        {
            if (string.IsNullOrEmpty(regName))
                return false;

            if (fInstancePlugIns.ContainsKey(regName))
                return true;
            else
                return base.Contains(regName);
        }

        protected bool RemoveInstancePlugIn(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", this);

            if (fInstancePlugIns.ContainsKey(regName))
            {
                lock (this)
                {
                    return fInstancePlugIns.Remove(regName);
                }
            }
            else
                return false;
        }

        internal override BaseRegItem GetRegItem(string regName)
        {
            InstanceRegItem item = ObjectUtil.TryGetValue(fInstancePlugIns, regName);
            if (item != null)
                return item;
            return base.GetRegItem(regName);
        }
    }
}
