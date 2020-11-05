using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public sealed class PlugInFactoryManager
    {
        private readonly Dictionary<string, BasePlugInFactory> fAllFactories;
        private readonly Dictionary<string, BaseXmlConfigFactory> fXmlConfigs;
        private readonly Dictionary<string, BasePlugInFactory> fCodeFactories;

        public PlugInFactoryManager()
        {
            fAllFactories = new Dictionary<string, BasePlugInFactory>();
            fXmlConfigs = new Dictionary<string, BaseXmlConfigFactory>();
            fCodeFactories = new Dictionary<string, BasePlugInFactory>();

            //Add(new SerializerPlugInFactory());
            //Add(new CacheItemCreatorPlugInFactory());
        }

        private BasePlugInFactory GetFactory(string name)
        {
            BasePlugInFactory factory = ObjectUtil.TryGetValue(fAllFactories, name);
            TkDebug.AssertNotNull(factory, string.Format(ObjectUtil.SysCulture,
                "没有找到名称为{0}的代码工厂", name), this);
            return factory;
        }

        internal IEnumerable<BasePlugInFactory> CodeFactories
        {
            get
            {
                return fCodeFactories.Values;
            }
        }

        internal IEnumerable<BaseXmlConfigFactory> XmlConfigs
        {
            get
            {
                return fXmlConfigs.Values;
            }
        }

        public void Add(BasePlugInFactory factory)
        {
            TkDebug.AssertArgumentNull(factory, "factory", this);

            lock (this)
            {
                TkDebug.Assert(!fAllFactories.ContainsKey(factory.Name), string.Format(ObjectUtil.SysCulture,
                    "{0}已经注册，请检查代码", factory.Name), factory);

                BaseXmlConfigFactory config = factory as BaseXmlConfigFactory;
                if (config != null)
                    fXmlConfigs.Add(config.Name, config);
                else
                    fCodeFactories.Add(factory.Name, factory);
                fAllFactories.Add(factory.Name, factory);
            }
        }

        public BaseXmlConfigFactory GetConfigFactory(string factoryName)
        {
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", this);

            BaseXmlConfigFactory result = ObjectUtil.TryGetValue(fXmlConfigs, factoryName);
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "没有找到名称为{0}的配置工厂", factoryName), this);
            return result;
        }

        public BasePlugInFactory GetCodeFactory(string factoryName)
        {
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", this);

            BasePlugInFactory result = ObjectUtil.TryGetValue(fCodeFactories, factoryName);
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "没有找到名称为{0}的代码插件工厂", factoryName), this);
            return result;
        }

        internal bool InternalAddCodePlugIn(BaseGlobalVariable globalVariable,
            BasePlugInAttribute attribute, Type type)
        {
            BasePlugInFactory factory = ObjectUtil.TryGetValue(fAllFactories, attribute.FactoryName);
            if (factory == null)
            {
                globalVariable.AddCodeError(attribute, type, PlugInErrorType.NoFactory);
                return false;
            }

            bool result = factory.Add(attribute, type);
            if (!result)
                globalVariable.AddCodeError(attribute, type, PlugInErrorType.Duplicate);
            else
                TkTrace.LogInfo($"在工程[{factory.Description}]中添加注册名为[{attribute.GetRegName(type)}]类型[{type}]");
            return result;
        }

        public bool AddCodePlugIn(BasePlugInAttribute attribute, Type type)
        {
            TkDebug.AssertArgumentNull(attribute, "attribute", this);
            TkDebug.AssertArgumentNull(type, "type", this);

            BasePlugInFactory factory = GetFactory(attribute.FactoryName);
            return factory.Add(attribute, type);
        }

        public static T CreateInstance<T>(string factoryName, string regName) where T : class
        {
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", null);
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            TkDebug.ThrowIfNoGlobalVariable();
            BasePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetFactory(factoryName);
            return factory.CreateInstance<T>(regName);
        }

        public static T CreateInstance<T>(string factoryName, string regName,
            params object[] args) where T : class
        {
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", null);
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            TkDebug.ThrowIfNoGlobalVariable();
            BasePlugInFactory factory = BaseGlobalVariable.Current.FactoryManager.GetFactory(factoryName);
            return factory.CreateInstance<T>(regName, args);
        }
    }
}