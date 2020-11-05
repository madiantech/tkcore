using System.ComponentModel;
using System.Drawing;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    internal class ColorTypeConverter : ITkTypeConverter
    {
        #region ITkTypeConverter 成员

        public object ConvertFromString(string text, ReadSettings settings)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Color));
            return converter.ConvertFromString(text);
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            if (value is Color)
                return ToString((Color)value);
            return string.Empty;
        }

        public string DefaultValue
        {
            get
            {
                return ToString(Color.Empty);
            }
        }

        #endregion ITkTypeConverter 成员

        private static string ToString(Color color)
        {
            int value = color.ToArgb();
            return value.ToString("X");
        }
    }
}