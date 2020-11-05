using System;

namespace YJC.Toolkit.Sys
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Struct
        | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class TkTypeConverterAttribute : Attribute
    {
        public TkTypeConverterAttribute(Type converterType)
        {
            TkDebug.AssertArgumentNull(converterType, "converterType", null);

            ConverterType = converterType;
        }

        public Type ConverterType { get; private set; }

        public bool UseObjectType { get; set; }

        public ITkTypeConverter CreateTypeConverter(Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            object result;
            if (UseObjectType)
                result = ObjectUtil.CreateObject(ConverterType, type);
            else
                result = ObjectUtil.CreateObject(ConverterType);

            return result.Convert<ITkTypeConverter>();
        }
    }
}