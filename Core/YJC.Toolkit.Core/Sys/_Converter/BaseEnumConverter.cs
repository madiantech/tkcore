using System;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseEnumConverter : ITkTypeConverter, IConvertStatus
    {
        private readonly Type fEnumType;
        private readonly bool fContainsFlag;

        protected BaseEnumConverter(Type enumType)
        {
            fEnumType = enumType;
            Attribute flags = Attribute.GetCustomAttribute(fEnumType, typeof(FlagsAttribute), false);
            fContainsFlag = flags != null;
        }

        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                string firstText = ObjectUtil.GetFirstEnumValue(fEnumType).ConvertToString();
                return ConvertEnumString(firstText);
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            try
            {
                IsSuccess = true;
                if (!fContainsFlag)
                    return Enum.Parse(fEnumType, text, true);
                else
                {
                    text = text.Replace(' ', ',');
                    return Enum.Parse(fEnumType, text, true);
                }
            }
            catch
            {
                IsSuccess = false;
                return ObjectUtil.GetFirstEnumValue(fEnumType);
            }
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            string result = value.ConvertToString();
            if (fContainsFlag)
                result = result.Replace(", ", " ");
            return ConvertEnumString(result);
        }

        #endregion

        #region IConvertStatus 成员

        public bool IsSuccess { get; private set; }
        #endregion

        protected virtual string ConvertEnumString(string input)
        {
            return input;
        }
    }
}
