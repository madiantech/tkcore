using System;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseDefaultValue : IDefaultValue, ICustomReader
    {
        public const string SIMPLE_NAME = "Simple";
        //public const string OBJECT_NAME = "DefaultObject";

        private readonly Dictionary<string, string> fFactories;
        private readonly Dictionary<string, Dictionary<string, object>> fFactoryValues;
        private readonly Dictionary<string, Type> fObjectConfig;
        private readonly Dictionary<string, object> fObjectValues;
        private readonly Dictionary<string, string> fSimpleDefaultValues;

        protected BaseDefaultValue()
        {
            fFactories = new Dictionary<string, string>();
            fFactoryValues = new Dictionary<string, Dictionary<string, object>>();
            fObjectConfig = new Dictionary<string, Type>();
            fObjectValues = new Dictionary<string, object>();
            fSimpleDefaultValues = new Dictionary<string, string>();
        }

        #region IDefaultValue 成员

        public Dictionary<string, object> GetFactorySection(string sectionName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, nameof(sectionName), this);

            var result = GetCustomSection(sectionName);
            if (result != null)
                return result;

            if (fFactoryValues.TryGetValue(sectionName, out result))
                return result;

            return null;
        }

        public object GetDefaultObject(string objectName)
        {
            TkDebug.AssertArgumentNullOrEmpty(objectName, nameof(objectName), this);

            if (fObjectValues.TryGetValue(objectName, out var result))
                return result;

            return null;
        }

        public object GetFactoryDefaultObject(string sectionName, string objectName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, nameof(sectionName), this);
            TkDebug.AssertArgumentNullOrEmpty(objectName, nameof(objectName), this);

            var result = GetCustomObject(sectionName, objectName);
            if (result != null)
                return result;

            var section = GetFactorySection(sectionName);
            if (section != null)
                if (section.TryGetValue(objectName, out result))
                    return result;

            return null;
        }

        public string GetSimpleDefaultValue(string keyName)
        {
            TkDebug.AssertArgumentNullOrEmpty(keyName, nameof(keyName), this);

            if (fSimpleDefaultValues.TryGetValue(keyName, out string result))
                return result;

            return null;
        }

        public void RegisterConfig(DefaultValueTypeFactory factory)
        {
            factory.EnumableCodePlugIn((regName, type, attribute) => TryAddSection(regName, type, attribute));
        }

        #endregion IDefaultValue 成员

        #region ICustomReader 成员

        public bool SupportVersion
        {
            get
            {
                return false;
            }
        }

        public CustomPropertyInfo CanRead(string localName, string version)
        {
            if (localName == SIMPLE_NAME)
            {
                CustomPropertyInfo result = new CustomPropertyInfo(typeof(string),
                    new DictionaryAttribute { NamespaceType = NamespaceType.Toolkit });
                return result;
            }
            else if (fFactories.ContainsKey(localName))
            {
                CustomPropertyInfo result = new CustomPropertyInfo(typeof(Dictionary<string, object>),
                    new DynamicDictionaryAttribute(fFactories[localName]));
                return result;
            }
            else if (fObjectConfig.ContainsKey(localName))
            {
                var attr = new ObjectElementAttribute
                {
                    ObjectType = fObjectConfig[localName]
                };
                return new CustomPropertyInfo(attr.ObjectType, attr);
            }
            return null;
        }

        public object GetValue(string localName, string version)
        {
            if (localName == SIMPLE_NAME)
                return fSimpleDefaultValues;
            else if (fFactoryValues.ContainsKey(localName))
                return fFactoryValues[localName];

            return null;
        }

        public void SetValue(string localName, string version, object value)
        {
            if (fObjectConfig.ContainsKey(localName))
                fObjectValues[localName] = value;
        }

        #endregion ICustomReader 成员

        private void TryAddSection(string regName, Type type, BasePlugInAttribute attribute)
        {
            if (attribute is FactoryDefaultValueAttribute factoryAttr)
                AddSection(factoryAttr.SectionName, factoryAttr.ConfigFactoryName);
            else if (attribute is DefaultValueAttribute)
                fObjectConfig.Add(regName, type);
        }

        protected virtual object GetCustomObject(string sectionName, string objectName)
        {
            return null;
        }

        protected virtual Dictionary<string, object> GetCustomSection(string sectionName)
        {
            return null;
        }

        public void AddSection(string sectionName, string factoryName)
        {
            TkDebug.AssertArgumentNullOrEmpty(sectionName, "sectionName", this);
            TkDebug.AssertArgumentNullOrEmpty(factoryName, "factoryName", this);

            if (!fFactories.ContainsKey(sectionName))
            {
                fFactories.Add(sectionName, factoryName);
                fFactoryValues.Add(sectionName, new Dictionary<string, object>());
            }
        }
    }
}