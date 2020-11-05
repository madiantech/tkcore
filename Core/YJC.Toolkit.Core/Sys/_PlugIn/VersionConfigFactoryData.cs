using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YJC.Toolkit.Sys
{
    internal class VersionConfigFactoryData : IConfigFactoryData
    {
        private class VersionData
        {
            private readonly Dictionary<string, ObjectElementAttribute> fElements;
            private ObjectElementAttribute fDefaultVersion;

            public VersionData()
            {
                fElements = new Dictionary<string, ObjectElementAttribute>();
            }

            public void Add(BaseObjectElementAttribute baseAttr, ObjectElementAttribute attr, string defaultVersion)
            {
                string version = string.IsNullOrEmpty(baseAttr.Version) ? defaultVersion : baseAttr.Version;
                if (!fElements.ContainsKey(version))
                {
                    fElements.Add(version, attr);
                    if (defaultVersion == version && fDefaultVersion != null)
                        fDefaultVersion = attr;
                }
            }

            public ObjectElementAttribute this[string version]
            {
                get
                {
                    if (string.IsNullOrEmpty(version))
                        return GetDefaultAttribute();
                    ObjectElementAttribute attr;
                    if (fElements.TryGetValue(version, out attr))
                        return attr;
                    return GetDefaultAttribute();
                }
            }

            public ObjectElementAttribute GetDefaultAttribute()
            {
                if (fDefaultVersion == null)
                    CalcDefaultVersion();
                return fDefaultVersion;
            }

            private void CalcDefaultVersion()
            {
                if (fElements.Count == 0)
                    return;
                string version = fElements.Min(item => item.Key);
                fDefaultVersion = fElements[version];
            }
        }

        private readonly Dictionary<QName, VersionData> fXmlElements;
        private readonly Dictionary<string, VersionData> fLocalElements;
        private readonly Dictionary<Type, ObjectElementAttribute> fTypeElements;

        public VersionConfigFactoryData()
        {
            fXmlElements = new Dictionary<QName, VersionData>();
            fLocalElements = new Dictionary<string, VersionData>();
            fTypeElements = new Dictionary<Type, ObjectElementAttribute>();
        }

        #region IConfigFactoryData 成员

        public bool SupportVersion
        {
            get
            {
                return true;
            }
        }

        public ObjectElementAttribute this[string name]
        {
            get
            {
                VersionData versionData;
                if (fLocalElements.TryGetValue(name, out versionData))
                    return versionData.GetDefaultAttribute();
                return null;
            }
        }

        public ObjectElementAttribute this[Type type]
        {
            get
            {
                ObjectElementAttribute attr;
                if (fTypeElements.TryGetValue(type, out attr))
                    return attr;
                return null;
            }
        }

        public void Add(BaseXmlConfigFactory factory, string regName, BaseObjectElementAttribute attr, Type type)
        {
            ObjectElementAttribute objAttr = new ObjectElementAttribute(attr.NamespaceType)
            {
                LocalName = regName,
                ObjectType = type,
                UseConstructor = attr.UseConstructor
            };
            if (attr.NamespaceType == NamespaceType.Namespace)
                objAttr.NamespaceUri = attr.NamespaceUri;
            QName qName = objAttr.GetQName(objAttr.LocalName);
            VersionData data = ObjectUtil.TryGetValue(fLocalElements, qName.LocalName);
            if (data == null)
            {
                data = new VersionData();
                fLocalElements.Add(qName.LocalName, data);
                fXmlElements.Add(qName, data);
            }
            data.Add(attr, objAttr, factory.DefaultVersion);
            fTypeElements.Add(type, objAttr);
        }

        public ObjectElementAttribute GetObjectElementAttribute(QName name, string version)
        {
            VersionData versionData;
            if (fXmlElements.TryGetValue(name, out versionData))
                return versionData[version];
            return null;
        }

        public ObjectElementAttribute GetObjectElementAttribute(string name, string version)
        {
            VersionData versionData;
            if (fLocalElements.TryGetValue(name, out versionData))
                return versionData[version];
            return null;
        }

        #endregion IConfigFactoryData 成员
    }
}