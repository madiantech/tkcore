using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseToolkitConfig : IConfig, ICustomReader
    {
        private readonly Dictionary<string, Type> fConfig;
        private readonly Dictionary<string, object> fData;

        protected BaseToolkitConfig()
        {
            fConfig = new Dictionary<string, Type>();
            fData = new Dictionary<string, object>();
        }

        public bool SupportVersion => false;

        public CustomPropertyInfo CanRead(string localName, string version)
        {
            if (fConfig.ContainsKey(localName))
            {
                var attr = new ObjectElementAttribute
                {
                    ObjectType = fConfig[localName]
                };
                return new CustomPropertyInfo(attr.ObjectType, attr);
            }

            return null;
        }

        public object GetConfig(string sectionName)
        {
            object result;
            if (fData.TryGetValue(sectionName, out result))
                return result;

            return null;
        }

        public object GetValue(string localName, string version)
        {
            return null;
        }

        public void RegisterConfig(ConfigTypeFactory factory)
        {
            factory.EnumableCodePlugIn((regName, type, attr) => fConfig.Add(regName, type));
        }

        public void SetValue(string localName, string version, object value)
        {
            fData[localName] = value;
        }
    }
}