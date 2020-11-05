using System;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal class ConfigFactoryElementReader : IMultipleElementReader, IElementWriter
    {
        private readonly IConfigFactoryData fConfigData;
        private readonly string fModelName;
        private readonly int fOrder;
        private readonly bool fMultiple;
        private readonly Type fCollectionType;
        private readonly bool fRequired;
        private readonly ObjectPropertyInfo fObjectInfo;
        private readonly bool fUseJsonObject;

        public ConfigFactoryElementReader(DynamicElementAttribute attr,
            ObjectPropertyInfo info, string modelName)
        {
            fModelName = modelName;
            fObjectInfo = info;
            fConfigData = attr.PlugInFactory.ConfigData;
            fOrder = attr.Order;
            fMultiple = attr.IsMultiple;
            fCollectionType = attr.CollectionType;
            fRequired = attr.Required;
            PropertyName = info.PropertyName;
            fUseJsonObject = attr.UseJsonObject;
        }

        public ConfigFactoryElementReader(DynamicDictionaryAttribute attr,
            ObjectPropertyInfo info, string modelName)
        {
            fModelName = modelName;
            fObjectInfo = info;
            fConfigData = attr.PlugInFactory.ConfigData;
            fOrder = attr.Order;
            fMultiple = false;
            fCollectionType = attr.CollectionType;
            fRequired = attr.Required;
            PropertyName = fObjectInfo.PropertyName;
            fUseJsonObject = attr.UseJsonObject;
        }

        #region IMultipleElementReader 成员

        public bool SupportVersion
        {
            get
            {
                return fConfigData.SupportVersion;
            }
        }

        public bool IsValueMulitple
        {
            get
            {
                return fMultiple;
            }
        }

        public ObjectPropertyInfo this[QName name, string version]
        {
            get
            {
                ObjectElementAttribute attr = fConfigData.GetObjectElementAttribute(name, version);
                if (attr == null)
                    return null;
                return CreatePropertyInfo(attr);
            }
        }

        public ObjectPropertyInfo this[QName name]
        {
            get
            {
                return this[name, null];
            }
        }

        public ObjectPropertyInfo this[string name]
        {
            get
            {
                ObjectElementAttribute attr = fConfigData[name];
                if (attr == null)
                    return null;
                return CreatePropertyInfo(attr);
            }
        }

        #endregion IMultipleElementReader 成员

        #region IElementWriter 成员

        public int Order
        {
            get
            {
                return fOrder;
            }
        }

        public bool IsSingle
        {
            get
            {
                return false;
            }
        }

        public ObjectPropertyInfo Content
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public bool Required
        {
            get
            {
                return fRequired;
            }
        }

        public string PropertyName { get; private set; }

        public ObjectPropertyInfo Get(Type type)
        {
            ObjectElementAttribute attr = fConfigData[type];
            if (attr == null)
                return null;

            return CreatePropertyInfo(attr);
        }

        public object GetValue(object receiver)
        {
            return fObjectInfo.GetValue(receiver);
        }

        #endregion IElementWriter 成员

        private ObjectPropertyInfo CreatePropertyInfo(ObjectElementAttribute attr)
        {
            ObjectElementAttribute newAttr = new ObjectElementAttribute(attr.NamespaceType)
            {
                LocalName = attr.LocalName,
                ObjectType = attr.ObjectType,
                UseConstructor = attr.UseConstructor,
                Order = fOrder,
                IsMultiple = fMultiple,
                CollectionType = fCollectionType,
                UseJsonObject = fUseJsonObject
            };
            if (attr.NamespaceType == NamespaceType.Namespace)
                newAttr.NamespaceUri = attr.NamespaceUri;
            ObjectPropertyInfo result = fObjectInfo.Clone(newAttr); //new ReflectorObjectPropertyInfo(fInfo, newAttr, fModelName);
            return result;
        }
    }
}