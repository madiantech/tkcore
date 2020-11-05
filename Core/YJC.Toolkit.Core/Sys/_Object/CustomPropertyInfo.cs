using System;

namespace YJC.Toolkit.Sys
{
    public sealed class CustomPropertyInfo
    {
        public CustomPropertyInfo(Type dataType, NamedAttribute attribute)
        {
            TkDebug.AssertArgumentNull(dataType, "dataType", null);
            TkDebug.AssertArgumentNull(attribute, "attribute", null);

            TkDebug.Assert(attribute is SimpleElementAttribute || attribute is BaseDictionaryAttribute,
                string.Format(ObjectUtil.SysCulture,
                "attribute必须是ElementAttribute或者DictionaryAttribute,现在的attribute是{0}",
                attribute.GetType()), null);

            DataType = dataType;
            Attribute = attribute;
        }

        public CustomPropertyInfo(Type dataType, NamedAttribute attribute, ITkTypeConverter converter)
            : this(dataType, attribute)
        {
            Converter = converter;
        }

        public Type DataType { get; private set; }

        public NamedAttribute Attribute { get; private set; }

        public ITkTypeConverter Converter { get; set; }
    }
}
