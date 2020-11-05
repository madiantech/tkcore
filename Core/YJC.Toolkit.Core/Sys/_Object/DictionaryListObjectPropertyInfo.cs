using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YJC.Toolkit.Sys
{
    internal class DictionaryListObjectPropertyInfo : ObjectPropertyInfo
    {
        private const string NAME = "Dictionary or List Internal";
        private readonly object fValue;
        private readonly Type fValueType;
        private readonly string fLocalName;

        public DictionaryListObjectPropertyInfo(object value, NamedAttribute attr)
            : base(attr, null)
        {
            fValue = value;
            fValueType = value.GetType();
            fLocalName = attr.LocalName;
        }

        public DictionaryListObjectPropertyInfo(object value, NamedAttribute attr, QName root)
            : this(value, attr)
        {
            if (root != null)
                fLocalName = root.LocalName;
        }

        public override Type DataType
        {
            get
            {
                return fValueType;
            }
        }

        public override string PropertyName
        {
            get
            {
                return NAME;
            }
        }

        public override void SetValue(object receiver, object value)
        {
        }

        public override object GetValue(object receiver)
        {
            return fValue;
        }

        public override string LocalName
        {
            get
            {
                return fLocalName;
            }
        }

        public override SerializerWriteMode WriteMode
        {
            get
            {
                return SerializerWriteMode.WriteNoName;
            }
        }

        public override ObjectPropertyInfo Clone(BaseObjectAttribute attribute)
        {
            throw new NotSupportedException();
        }

        public override Type ObjectType
        {
            get
            {
                return Attribute.GetObjType(fValueType, NAME);
            }
        }
    }
}