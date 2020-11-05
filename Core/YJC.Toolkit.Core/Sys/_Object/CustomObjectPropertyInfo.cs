using System;

namespace YJC.Toolkit.Sys
{
    internal class CustomObjectPropertyInfo : ObjectPropertyInfo
    {
        private readonly string fLocalName;
        private readonly ICustomReader fReader;
        private readonly Type fType;
        private readonly string fVersion;
        private readonly QName fQName;

        public CustomObjectPropertyInfo(string localName, ICustomReader reader,
            CustomPropertyInfo input, string version, string modelName)
            : base(input.Attribute, modelName)
        {
            fVersion = version;
            fLocalName = localName;
            fReader = reader;
            fType = input.DataType;
            Converter = input.Converter;
            fQName = CreateQName(fLocalName);
        }

        private CustomObjectPropertyInfo(CustomObjectPropertyInfo source,
            BaseObjectAttribute attribute)
            : base(attribute, source.ModelName)
        {
            fVersion = source.fVersion;
            fLocalName = source.fLocalName;
            fReader = source.fReader;
            fType = source.fType;
            Converter = source.Converter;
            fQName = CreateQName(fLocalName);
        }

        public override Type DataType
        {
            get
            {
                return fType;
            }
        }

        public override string PropertyName
        {
            get
            {
                return fLocalName;
            }
        }

        public override string LocalName
        {
            get
            {
                return fLocalName;
            }
        }

        public override Type ObjectType
        {
            get
            {
                return fType;
            }
        }

        public override QName QName => fQName;

        public override SerializerWriteMode WriteMode
        {
            get
            {
                return SerializerWriteMode.None;
            }
        }

        private QName CreateQName(string localName)
        {
            var result = QName.Get(localName);
            result.IgnoreNamespace = true;
            return result;
        }

        public override ObjectPropertyInfo Clone(BaseObjectAttribute attribute)
        {
            return new CustomObjectPropertyInfo(this, attribute);
        }

        public override void SetValue(object receiver, object value)
        {
            fReader.SetValue(fLocalName, fVersion, value);
        }

        public override object GetValue(object receiver)
        {
            return fReader.GetValue(fLocalName, fVersion);
        }
    }
}