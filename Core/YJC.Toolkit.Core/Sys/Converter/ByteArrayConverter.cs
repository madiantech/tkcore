using System;

namespace YJC.Toolkit.Sys.Converter
{
    class ByteArrayConverter : ITkTypeConverter
    {
        public static readonly ITkTypeConverter Converter = new ByteArrayConverter();

        /// <summary>
        /// Initializes a new instance of the ByteArrayConverter class.
        /// </summary>
        private ByteArrayConverter()
        {
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
            try
            {
                return Convert.FromBase64String(text);
            }
            catch
            {
                return null;
            }
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            try
            {
                byte[] data = value as byte[];
                if (data != null)
                    return Convert.ToBase64String(data);
                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion

    }
}
