using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    internal class ToolkitDynObject : ICustomReader
    {
        private string fLocalName;
        private string fVersion;
        private readonly IConfigFactoryData fConfigData;

        public ToolkitDynObject(string factoryName)
        {
            BaseXmlConfigFactory factory = BaseGlobalVariable.Current.FactoryManager.GetConfigFactory(
                factoryName);
            fConfigData = factory.ConfigData;
        }

        #region ICustomReader 成员

        public bool SupportVersion
        {
            get
            {
                return fConfigData.SupportVersion;
            }
        }

        public CustomPropertyInfo CanRead(string localName, string version)
        {
            ObjectElementAttribute attr = fConfigData.GetObjectElementAttribute(localName, version);
            if (attr != null)
            {
                fLocalName = localName;
                fVersion = version;
                return new CustomPropertyInfo(attr.ObjectType, attr);
            }
            return null;
        }

        public object GetValue(string localName, string version)
        {
            return null;
        }

        public void SetValue(string localName, string version, object value)
        {
            if (SupportVersion)
            {
                if (localName == fLocalName && version == fVersion)
                    Data = value;
            }
            else
            {
                if (localName == fLocalName)
                    Data = value;
            }
        }

        #endregion ICustomReader 成员

        public object Data { get; private set; }
    }
}