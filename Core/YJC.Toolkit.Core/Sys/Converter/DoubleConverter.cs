using System;
using System.Globalization;

namespace YJC.Toolkit.Sys.Converter
{
    class DoubleConverter : BaseTypeConverter<double>
    {
        public static readonly ITkTypeConverter Converter = new DoubleConverter();
        internal static readonly NumberStyles Styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands;

        /// <summary>
        /// Initializes a new instance of the DoubleConverter class.
        /// </summary>
        private DoubleConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            double result;
            if (double.TryParse(text, Styles, ObjectUtil.SysCulture, out result))
                return result;
            return Convert.ToDouble(text, ObjectUtil.SysCulture);
        }
    }
}
