using System;
using System.Collections.Generic;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal class ConfigFactoryData : IConfigFactoryData
    {
        private readonly Dictionary<QName, ObjectElementAttribute> fXmlElements;
        private readonly Dictionary<string, ObjectElementAttribute> fLocalElements;
        private readonly Dictionary<Type, ObjectElementAttribute> fTypeElements;

        public ConfigFactoryData()
        {
            fXmlElements = new Dictionary<QName, ObjectElementAttribute>();
            fLocalElements = new Dictionary<string, ObjectElementAttribute>();
            fTypeElements = new Dictionary<Type, ObjectElementAttribute>();
        }

        public void Add(ObjectElementAttribute attr, PropertyInfo info)
        {
            TkDebug.AssertNotNullOrEmpty(attr.LocalName, string.Format(ObjectUtil.SysCulture,
              "属性{0}由于配置多个ObjectElement，因此需要指定LocalName", info.Name), attr);
            TkDebug.AssertNotNull(attr.ObjectType, string.Format(ObjectUtil.SysCulture,
                "属性{0}由于配置多个ObjectElement，因此需要指定LocalName", info.Name), attr);
            QName name = attr.GetQName(attr.LocalName);
            fXmlElements.Add(name, attr);
            fLocalElements.Add(name.LocalName, attr);
            fTypeElements.Add(attr.ObjectType, attr);
        }

        public ObjectElementAttribute this[QName name]
        {
            get
            {
                ObjectElementAttribute attr;
                if (fXmlElements.TryGetValue(name, out attr))
                    return attr;
                return null;
            }
        }

        #region IConfigFactoryData 成员

        public bool SupportVersion
        {
            get
            {
                return false;
            }
        }

        public ObjectElementAttribute this[string name]
        {
            get
            {
                ObjectElementAttribute attr;
                if (fLocalElements.TryGetValue(name, out attr))
                    return attr;
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

        public ObjectElementAttribute GetObjectElementAttribute(QName name, string version)
        {
            ObjectElementAttribute attr;
            if (fXmlElements.TryGetValue(name, out attr))
                return attr;
            return null;
        }

        public ObjectElementAttribute GetObjectElementAttribute(string name, string version)
        {
            return this[name];
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
            fXmlElements.Add(qName, objAttr);
            fLocalElements.Add(qName.LocalName, objAttr);
            fTypeElements.Add(type, objAttr);
        }

        #endregion IConfigFactoryData 成员
    }
}