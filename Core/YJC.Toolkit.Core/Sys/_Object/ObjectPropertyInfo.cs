using System;

namespace YJC.Toolkit.Sys
{
    public abstract class ObjectPropertyInfo
    {
        protected ObjectPropertyInfo(BaseObjectAttribute attribute, string modelName)
        {
            TkDebug.AssertArgumentNull(attribute, "attribute", null);

            Attribute = attribute;
            ModelName = modelName;
        }

        public BaseObjectAttribute Attribute { get; private set; }

        public string ModelName { get; private set; }

        public abstract Type DataType { get; }

        public ITkTypeConverter Converter { get; protected set; }

        public abstract string PropertyName { get; }

        public abstract void SetValue(object receiver, object value);

        public abstract object GetValue(object receiver);

        public abstract string LocalName { get; }

        public abstract SerializerWriteMode WriteMode { get; }

        public virtual QName QName
        {
            get
            {
                string localName = LocalName;
                NamedAttribute attr = Attribute.Convert<NamedAttribute>();
                switch (attr.NamespaceType)
                {
                    case NamespaceType.None:
                        return QName.Get(localName);

                    default:
                        return QName.Get(localName, attr.NamespaceUri);
                }
            }
        }

        public abstract ObjectPropertyInfo Clone(BaseObjectAttribute attribute);

        public abstract Type ObjectType { get; }

        public override string ToString()
        {
            return LocalName ?? base.ToString();
        }
    }
}