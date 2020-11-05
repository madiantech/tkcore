using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using YJC.Toolkit.Sys.Converter;

namespace YJC.Toolkit.Sys
{
    public static class TkTypeDescriptor
    {
        private readonly static Dictionary<Type, ITkTypeConverter> fMaps = CreateMap();

        private static Dictionary<Type, ITkTypeConverter> CreateMap()
        {
            Dictionary<Type, ITkTypeConverter> map = new Dictionary<Type, ITkTypeConverter>();
            map.Add(typeof(int), IntConverter.Converter);
            map.Add(typeof(string), StringConverter.Converter);
            map.Add(typeof(short), ShortConverter.Converter);
            map.Add(typeof(long), LongConverter.Converter);
            map.Add(typeof(float), FloatConverter.Converter);
            map.Add(typeof(double), DoubleConverter.Converter);
            map.Add(typeof(DateTime), DateTimeConverter.Converter);
            map.Add(typeof(Guid), GuidConverter.Converter);
            map.Add(typeof(bool), BoolConverter.Converter);
            map.Add(typeof(byte[]), ByteArrayConverter.Converter);
            map.Add(typeof(string[]), StringArrayConverter.Converter);
            map.Add(typeof(TimeSpan), TimeSpanConverter.Converter);
            map.Add(typeof(Encoding), EncodingConverter.Converter);
            map.Add(typeof(CultureInfo), CultureInfoConverter.Converter);
            //map.Add(typeof(), Converter.Converter);
            return map;
        }

        public static ITkTypeConverter GetSimpleConverter(Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            // 枚举类型
            if (type.IsEnum)
                return new EnumConverter(type);
            // nullable类型
            if (type.IsNullableType())
                return new InternalNullableConverter(type);

            ITkTypeConverter result;
            if (fMaps.TryGetValue(type, out result))
                return result;

            Attribute attr = Attribute.GetCustomAttribute(type, typeof(TkTypeConverterAttribute));
            if (attr != null)
                return attr.Convert<TkTypeConverterAttribute>().CreateTypeConverter(type);

            return null;
        }

        public static ITkTypeConverter GetConverter(Type type)
        {
            ITkTypeConverter result = GetSimpleConverter(type);
            if (result != null)
                return result;

            if (BaseGlobalVariable.Current != null)
            {
                result = BaseGlobalVariable.Current.GetExTypeConverter(type);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static ITkTypeConverter GetConverter(PropertyInfo property, Type propertyType)
        {
            TkDebug.AssertArgumentNull(property, "property", null);
            TkDebug.AssertArgumentNull(propertyType, "propertyType", null);

            Attribute attr = Attribute.GetCustomAttribute(property, typeof(TkTypeConverterAttribute));
            if (attr != null)
                return attr.Convert<TkTypeConverterAttribute>().CreateTypeConverter(propertyType);

            if (BaseGlobalVariable.Current != null)
            {
                ITkTypeConverter result = BaseGlobalVariable.Current.GetExTypeConverter(property);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
