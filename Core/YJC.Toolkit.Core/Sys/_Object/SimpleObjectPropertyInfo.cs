using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Sys
{
    internal class SimpleObjectPropertyInfo : ObjectPropertyInfo
    {
        private const string NAME = "Simple Value Internal";
        private readonly object fValue;
        private readonly Type fValueType;
        private readonly string fLocalName;

        public SimpleObjectPropertyInfo(object value, Type valueType,
            SimpleElementAttribute attribute, ITkTypeConverter converter)
            : base(attribute, null)
        {
            fValue = value;
            fValueType = value.GetType();
            fLocalName = attribute.LocalName;
            Converter = converter;
        }

        public override Type DataType { get => fValueType; }

        public override string PropertyName { get => NAME; }

        public override string LocalName { get => fLocalName; }

        public override SerializerWriteMode WriteMode { get => SerializerWriteMode.WriteNoName; }

        public override Type ObjectType { get => Attribute.GetObjType(fValueType, NAME); }

        public override ObjectPropertyInfo Clone(BaseObjectAttribute attribute)
        {
            throw new NotSupportedException();
        }

        public override object GetValue(object receiver)
        {
            return fValue;
        }

        public override void SetValue(object receiver, object value)
        {
        }
    }
}