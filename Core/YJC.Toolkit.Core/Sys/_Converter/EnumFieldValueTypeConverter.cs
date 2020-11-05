using System;
using System.Collections.Generic;
using System.Reflection;

namespace YJC.Toolkit.Sys
{
    public sealed class EnumFieldValueTypeConverter : ITkTypeConverter
    {
        private readonly Dictionary<object, string> fValues;
        private readonly Dictionary<string, object> fNames;
        private readonly Type fEnumType;
        private readonly Tuple<string, object> fFirstValue;

        public EnumFieldValueTypeConverter(Type enumType)
        {
            TkDebug.AssertArgumentNull(enumType, "enumType", null);

            fEnumType = enumType;
            fNames = new Dictionary<string, object>();
            fValues = new Dictionary<object, string>();

            Type descType = typeof(EnumFieldValueAttribute);
            foreach (FieldInfo field in fEnumType.GetFields())
            {
                //过滤掉一个不是枚举值的，记录的是枚举的源类型
                if (!field.FieldType.IsEnum)
                    continue;

                var attributes = field.GetCustomAttributes(descType, false);
                if (attributes.Length > 0)
                {
                    object value = Enum.Parse(fEnumType, field.Name, true);
                    EnumFieldValueAttribute temp = (EnumFieldValueAttribute)attributes[0];
                    fValues.Add(value, temp.Value);
                    fNames[temp.Value] = value;

                    if (fFirstValue == null)
                        fFirstValue = Tuple.Create(temp.Value, value);
                }
            }
        }

        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                if (fFirstValue != null)
                    return fFirstValue.Item1;
                return null;
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            object value;
            if (string.IsNullOrEmpty(text))
                return fFirstValue.Item2;
            text = text.Trim();
            if (fNames.TryGetValue(text, out value))
                return value;
            return fFirstValue.Item2;
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            string text;
            if (fValues.TryGetValue(value, out text))
                return text;
            return fFirstValue.Item1;
        }

        #endregion ITkTypeConverter 成员
    }
}