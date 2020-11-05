using System;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    internal sealed class ReflectorObjectPropertyInfo : ObjectPropertyInfo
    {
        private readonly string fPascalName;
        private readonly string fCamelName;
        private readonly PropertyInfo fProperty;

        public ReflectorObjectPropertyInfo(PropertyInfo property,
            BaseObjectAttribute attribute, string modelName)
            : base(attribute, modelName)
        {
            fProperty = property;
            fPascalName = property.Name;
            fCamelName = char.ToLowerInvariant(fPascalName[0]) + fPascalName.Substring(1);
            Converter = TkTypeDescriptor.GetConverter(property,
                attribute.GetObjType(property.PropertyType, property.Name));
        }

        public override Type DataType
        {
            get
            {
                return fProperty.PropertyType;
            }
        }

        public override string PropertyName
        {
            get
            {
                return fProperty.Name;
            }
        }

        public PropertyInfo Property
        {
            get
            {
                return fProperty;
            }
        }

        public override void SetValue(object receiver, object value)
        {
            ObjectUtil.SetValue(fProperty, receiver, value);
        }

        public override object GetValue(object receiver)
        {
            return ObjectUtil.GetValue(fProperty, receiver);
        }

        public override string LocalName
        {
            get
            {
                NamedAttribute attr = Attribute as NamedAttribute;
                TkDebug.AssertNotNull(attr, string.Format(ObjectUtil.SysCulture,
                    "当前的Attribute({0})不支持LocalName", attr), this);

                if (string.IsNullOrEmpty(attr.LocalName))
                {
                    switch (attr.NamingRule)
                    {
                        case NamingRule.Pascal:
                            return fPascalName;

                        case NamingRule.Camel:
                            return fCamelName;

                        case NamingRule.Upper:
                            return fPascalName.ToUpper();

                        case NamingRule.Lower:
                            return fPascalName.ToLower();

                        case NamingRule.UnderLineLower:
                            return StringUtil.GetUnderlineLowerName(fPascalName);

                        default:
                            TkDebug.ThrowImpossibleCode(this);
                            return string.Empty;
                    }
                }
                else
                    return attr.LocalName;
            }
        }

        public override Type ObjectType
        {
            get
            {
                return Attribute.GetObjType(fProperty.PropertyType, fProperty.Name);
            }
        }

        public override SerializerWriteMode WriteMode
        {
            get
            {
                return SerializerWriteMode.WriteName;
            }
        }

        public override ObjectPropertyInfo Clone(BaseObjectAttribute attribute)
        {
            return new ReflectorObjectPropertyInfo(fProperty, attribute, ModelName);
        }
    }
}