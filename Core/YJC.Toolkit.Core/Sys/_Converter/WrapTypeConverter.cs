using System.ComponentModel;

namespace YJC.Toolkit.Sys
{
    class WrapTypeConverter : ITkTypeConverter
    {
        private readonly TypeConverter fConverter;

        public WrapTypeConverter(TypeConverter converter)
        {
            fConverter = converter;
        }

        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                return null;
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            return fConverter.ConvertFromString(text);
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            return fConverter.ConvertToString(value);
        }

        #endregion
    }
}
