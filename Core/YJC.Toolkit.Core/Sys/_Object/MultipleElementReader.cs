using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal class MultipleElementReader : IMultipleElementReader, IElementWriter
    {
        private readonly ConfigFactoryData fConfigData;
        private readonly PropertyInfo fInfo;
        private readonly string fModelName;

        public MultipleElementReader(PropertyInfo info, string modelName,
            IEnumerable<ObjectElementAttribute> attrs)
        {
            fModelName = modelName;
            fInfo = info;
            fConfigData = new ConfigFactoryData();
            foreach (var item in attrs)
            {
                item.SetNameMode(modelName, info);
                fConfigData.Add(item, info);
            }
            Required = attrs.All(attr => attr.Required);
            Order = attrs.Min(attr => attr.Order);
            IsValueMulitple = attrs.Any(attr => attr.IsMultiple);
            PropertyName = fInfo.Name;
        }

        #region IMultipleElementReader 成员

        public bool SupportVersion
        {
            get
            {
                return false;
            }
        }

        public bool IsValueMulitple { get; private set; }

        public ObjectPropertyInfo this[QName name, string version]
        {
            get
            {
                return this[name];
            }
        }

        public ObjectPropertyInfo this[QName name]
        {
            get
            {
                ObjectElementAttribute attr = fConfigData[name];
                if (attr == null)
                    return null;
                return CreatePropertyInfo(attr);
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

        public int Order { get; private set; }

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

        public bool Required { get; private set; }
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
            return ObjectUtil.GetValue(fInfo, receiver);
        }

        #endregion IElementWriter 成员

        private ObjectPropertyInfo CreatePropertyInfo(ObjectElementAttribute attr)
        {
            ObjectPropertyInfo result = new ReflectorObjectPropertyInfo(fInfo, attr, fModelName);
            return result;
        }
    }
}