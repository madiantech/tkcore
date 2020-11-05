using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    class EnumCodeRegItem : CodeRegItem
    {
        private readonly EnumCodeTableAttribute fAttribute;

        public EnumCodeRegItem(string regName, BasePlugInAttribute attribute, Type regType)
            : base(regName, attribute, regType)
        {
            fAttribute = attribute.Convert<EnumCodeTableAttribute>();
            fAttribute.RegName = regName;
        }

        public override T CreateInstance<T>()
        {
            var result = new EnumCodeTable(RegType, fAttribute.UseIntValue);
            result.SetAttribute(fAttribute);
            return result.Convert<T>();
        }
    }
}
