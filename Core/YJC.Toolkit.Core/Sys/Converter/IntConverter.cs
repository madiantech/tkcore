using System;

namespace YJC.Toolkit.Sys.Converter
{
    class IntConverter : BaseTypeConverter<int>
    {
        public static readonly ITkTypeConverter Converter = new IntConverter();

        /// <summary>
        /// Initializes a new instance of the IntConverter class.
        /// </summary>
        private IntConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            if (text == null)
                return 0;
            if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                return Convert.ToInt32(text, 16);
            int result;
            if (int.TryParse(text, DoubleConverter.Styles, ObjectUtil.SysCulture, out result))
                return result;
            return Convert.ToInt32(text, ObjectUtil.SysCulture);
        }
    }
}
