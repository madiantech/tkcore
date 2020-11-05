using System;

namespace YJC.Toolkit.Sys
{
    public sealed class EnumIntTypeConverter : ITkTypeConverter
    {
        private readonly Type fEnumType;
        private readonly object fDefaultValue;

        public EnumIntTypeConverter(Type enumType)
        {
            TkDebug.AssertArgumentNull(enumType, "enumType", null);

            fEnumType = enumType;
            var values = fEnumType.GetEnumValues();
            if (values.Length > 0)
                fDefaultValue = values.GetValue(0);
        }

        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                if (fDefaultValue != null)
                    return ((int)fDefaultValue).ToString(ObjectUtil.SysCulture);

                return null;
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            int value = text.Value<int>();
            return Enum.ToObject(fEnumType, value);
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            try
            {
                int intValue = (int)value;
                return intValue.ToString(ObjectUtil.SysCulture);
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
