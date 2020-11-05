using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace YJC.Toolkit.Sys
{
    public static partial class ObjectUtil
    {
        public static CultureInfo SysCulture
        {
            get
            {
                if (BaseAppSetting.Current != null)
                    return BaseAppSetting.Current.Culture ?? CultureInfo.CurrentCulture;
                else
                    return CultureInfo.CurrentCulture;
            }
        }

        public static ReadSettings ReadSettings
        {
            get
            {
                if (BaseAppSetting.Current != null)
                    return BaseAppSetting.Current.ReadSettings ?? ReadSettings.Default;
                else
                    return ReadSettings.Default;
            }
        }

        public static WriteSettings WriteSettings
        {
            get
            {
                if (BaseAppSetting.Current != null)
                    return BaseAppSetting.Current.WriteSettings ?? WriteSettings.Default;
                else
                    return WriteSettings.Default;
            }
        }

        internal static void AssertTypeConverter(object sender, Type type,
             ITkTypeConverter converter)
        {
            TkDebug.AssertNotNull(converter, string.Format(ObjectUtil.SysCulture,
                "无法获取类型{0}的TypeConverter，请确认是否为其配置TkTypeConverterAttribute",
                type), sender);
        }

        internal static void AssertTypeConverter(object sender, Type type,
            ITkTypeConverter converter, string propertyName)
        {
            TkDebug.AssertNotNull(converter, string.Format(ObjectUtil.SysCulture,
                "在分析属性{1}时出错：无法获取类型{0}的TypeConverter，请确认是否为其配置TkTypeConverterAttribute",
                type, propertyName), sender);
        }

        private static object GetDefaultValue(Type type, object defaultValue,
            ITkTypeConverter converter, ReadSettings settings)
        {
            if (defaultValue != null)
            {
                if (ObjectUtil.IsSubType(type, defaultValue.GetType()))
                    return defaultValue;
                else
                {
                    try
                    {
                        string value = defaultValue.ToString();
                        return converter.ConvertFromString(value, settings);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (type.IsEnum)
                {
                    return GetFirstEnumValue(type);
                }
                else if (type.IsValueType)
                {
                    return ObjectUtil.CreateObject(type);
                }
                else
                    return null;
            }
        }

        public static T GetDefaultValue<T>(string strValue)
        {
            Type type = typeof(T);
            if (type.IsEnum)
                return GetDefaultValue(strValue, (T)GetFirstEnumValue(type));
            else
                return GetDefaultValue(strValue, default(T));
        }

        public static object GetDefaultValue(string strValue, Type type)
        {
            TkDebug.AssertArgumentNull(type, "type", null);

            if (type.IsEnum)
                return GetDefaultValue(strValue, GetFirstEnumValue(type));
            else
                return InternalGetDefaultValue(type, strValue, GetTypeDefaultValue(type), ReadSettings.Default, true);
        }

        public static T GetDefaultValue<T>(string strValue, T defaultValue)
        {
            return InternalGetDefaultValue<T>(strValue, defaultValue, ReadSettings.Default, true);
        }

        /// <summary>
        /// 获取枚举类型的第一个值
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object GetFirstEnumValue(Type enumType)
        {
            TkDebug.AssertArgumentNull(enumType, "enumType", null);

            Array values = Enum.GetValues(enumType);
            TkDebug.Assert(values.Length > 0, string.Format(ObjectUtil.SysCulture,
                "枚举类型{0}中没有枚举值", enumType), null);
            return (values as IList)[0];
        }

        public static object GetValue(object sender, Type type, string strValue,
            object defaultValue, ReadSettings settings)
        {
            TkDebug.AssertArgumentNull(type, "type", sender);

            ITkTypeConverter converter = TkTypeDescriptor.GetConverter(type);
            AssertTypeConverter(sender, type, converter);
            return InternalGetValue(type, strValue, defaultValue, settings, converter);
        }

        private static T InternalGetDefaultValue<T>(T defaultValue)
        {
            if (defaultValue != null)
                return defaultValue;
            else
            {
                Type type = typeof(T);
                if (type.IsEnum)
                    return (T)GetFirstEnumValue(type);
                else
                    return default(T);
            }
        }

        private static object InternalGetDefaultValue(Type type, object defaultValue)
        {
            if (defaultValue != null)
                return defaultValue;
            else
                return GetTypeDefaultValue(type);
        }

        internal static object GetTypeDefaultValue(Type type)
        {
            if (type.IsEnum)
                return GetFirstEnumValue(type);
            else if (type.IsValueType)
                return Activator.CreateInstance(type);
            else
                return null;
        }

        internal static T InternalGetDefaultValue<T>(string strValue, T defaultValue,
            ReadSettings settings, bool throwException)
        {
            Type type = typeof(T);
            ITkTypeConverter converter = TkTypeDescriptor.GetConverter(type);
            if (throwException)
                TkDebug.AssertNotNull(converter, string.Format(ObjectUtil.SysCulture,
                    "无法获取类型{0}的TypeConverter，请确认是否为其配置TypeConverterAttribute",
                    type), null);
            else
            {
                if (converter == null)
                    return default(T);
            }
            try
            {
                return strValue == null ? InternalGetDefaultValue(defaultValue)
                    : (T)converter.ConvertFromString(strValue, settings);
            }
            catch
            {
                return InternalGetDefaultValue(defaultValue);
            }
        }

        internal static object InternalGetDefaultValue(Type type, string strValue, object defaultValue,
            ReadSettings settings, bool throwException)
        {
            ITkTypeConverter converter = TkTypeDescriptor.GetConverter(type);
            if (throwException)
                TkDebug.AssertNotNull(converter, string.Format(ObjectUtil.SysCulture,
                    "无法获取类型{0}的TypeConverter，请确认是否为其配置TypeConverterAttribute",
                    type), null);
            else
            {
                if (converter == null)
                    return GetTypeDefaultValue(type);
            }
            try
            {
                return strValue == null ? InternalGetDefaultValue(type, defaultValue)
                    : converter.ConvertFromString(strValue, settings);
            }
            catch
            {
                return InternalGetDefaultValue(type, defaultValue);
            }
        }

        internal static object InternalGetValue(Type type, string strValue,
            object defaultValue, ReadSettings settings, ITkTypeConverter converter)
        {
            try
            {
                return strValue == null ? GetDefaultValue(type, defaultValue, converter, settings)
                    : converter.ConvertFromString(strValue, settings);
            }
            catch
            {
                return GetDefaultValue(type, defaultValue, converter, settings);
            }
        }

        /// <summary>
        /// 判断输入值和缺省值是否相同
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefaultValue(object defaultValue, object value)
        {
            if (defaultValue != null)
            {
                if (defaultValue == value)
                    return true;
            }
            else
            {
                if (value == null)
                    return true;
                Type type = value.GetType();
                if (type.IsValueType)
                {
                    try
                    {
                        object nullValue = Convert.ChangeType(0, type, ObjectUtil.SysCulture);
                        if (value.Equals(nullValue))
                            return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        internal static string ToString(ITkTypeConverter converter, object obj, WriteSettings settings)
        {
            if (obj == null)
                return string.Empty;

            if (converter == null)
                converter = TkTypeDescriptor.GetConverter(obj.GetType());
            if (converter != null)
                return converter.ConvertToString(obj, settings);
            else
                return obj.ToString();
        }

        public static string ToString(object obj, WriteSettings settings)
        {
            if (obj == null)
                return string.Empty;

            ITkTypeConverter converter = TkTypeDescriptor.GetConverter(obj.GetType());
            if (converter != null)
                return converter.ConvertToString(obj, settings);
            else
                return obj.ToString();
        }

        public static T TryGetValue<T>(Dictionary<string, T> dictionary, string key)
        {
            TkDebug.AssertArgumentNull(dictionary, "dictionary", null);
            TkDebug.AssertArgumentNull(key, "key", null);

            T result;
            return dictionary.TryGetValue(key, out result) ? result : default(T);
        }

        public static T ParseEnum<T>(string value, bool ignoreCase) where T : struct
        {
            TkDebug.AssertArgumentNullOrEmpty(value, "value", null);

            Type enumType = typeof(T);
            TkDebug.Assert(enumType.IsEnum, string.Format(ObjectUtil.SysCulture,
                "参数T的类型不是枚举类型，而是{0}", enumType), null);
            return (T)Enum.Parse(enumType, value, ignoreCase);
        }

        public static T ParseEnum<T>(string value) where T : struct
        {
            return ParseEnum<T>(value, false);
        }

        public static T? TryParseEnum<T>(string value, bool ignoreCase) where T : struct
        {
            try
            {
                return ParseEnum<T>(value, ignoreCase);
            }
            catch
            {
                return null;
            }
        }

        public static bool Equals<T>(T left, T right) where T : class
        {
            if (object.Equals(left, null) && object.Equals(right, null))
                return true;
            if (object.Equals(left, null) || object.Equals(right, null))
                return false;

            return left.Equals(right);
        }

        public static T QueryObject<T>(params object[] args) where T : class
        {
            if (args == null)
                return null;

            foreach (object item in args)
            {
                T result = item as T;
                if (result != null)
                    return result;
            }
            return null;
        }

        public static T ConfirmQueryObject<T>(object owner, params object[] args) where T : class
        {
            T result = QueryObject<T>(args);
            TkDebug.AssertNotNull(result, string.Format(ObjectUtil.SysCulture,
                "参数中没有包含类型为{0}的对象，请确认", typeof(T)), owner);

            return result;
        }

        public static T[] CopyArray<T>(T[] source)
        {
            if (source == null)
                return null;
            T[] result = new T[source.Length];
            Array.Copy(source, result, source.Length);
            return result;
        }

        public static bool ArrayEqual<T>(T[] array1, T[] array2)
        {
            return ArrayEqual(array1, array2, EqualityComparer<T>.Default);
        }

        public static bool ArrayEqual<T>(T[] array1, T[] array2, IEqualityComparer<T> comparer)
        {
            if (array1 == null && array2 == null)
                return true;
            if (array1 == null || array2 == null)
                return false;

            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (!comparer.Equals(array1[i], array2[i]))
                    return false;
            }
            return true;
        }

        internal static Type GetDictionaryValueType(Type propertyType, string name, object sender)
        {
            Type type = propertyType;
            TkDebug.Assert(type.IsGenericType, string.Format(ObjectUtil.SysCulture,
                "无法从属性{0}的类型中获取ObjectType，因为其不是泛型类型", name), sender);
            Type[] args = type.GetGenericArguments();
            TkDebug.Assert(args.Length == 2, string.Format(ObjectUtil.SysCulture,
                "类型{0}的泛型参数个数不为2，无法获取第二个泛型参数", type), sender);
            return args[1];
        }

        internal static Type GetListValueType(Type propertyType, string name, object sender)
        {
            Type type = propertyType;
            TkDebug.Assert(type.IsGenericType, string.Format(ObjectUtil.SysCulture,
                "无法从属性{0}的类型中获取ObjectType，因为其不是泛型类型", name), sender);
            Type[] args = type.GetGenericArguments();
            TkDebug.Assert(args.Length == 1, string.Format(ObjectUtil.SysCulture,
               "类型{0}的泛型参数个数不为1，无法获取第一个泛型参数", type), sender);
            return args[0];
        }
    }
}