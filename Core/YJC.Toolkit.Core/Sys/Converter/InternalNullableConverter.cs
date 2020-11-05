using System;

namespace YJC.Toolkit.Sys.Converter
{
    class InternalNullableConverter : NullableConverter
    {
        public InternalNullableConverter(Type type)
            : base(CreateConverter(type))
        {
        }

        private static ITkTypeConverter CreateConverter(Type type)
        {
            Type innerType = type.GetGenericArguments()[0];
            var result = TkTypeDescriptor.GetConverter(innerType);
            return result;
        }
    }
}
