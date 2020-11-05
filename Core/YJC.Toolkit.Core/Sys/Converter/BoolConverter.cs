using System;

namespace YJC.Toolkit.Sys.Converter
{
    class BoolConverter : BaseTypeConverter<bool>
    {
        public static readonly ITkTypeConverter Converter = new BoolConverter();

        private BoolConverter()
        {

        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return Convert.ToBoolean(text, ObjectUtil.SysCulture);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public override string ConvertToString(object value, WriteSettings settings)
        {
            return value.ConvertToString().ToLowerInvariant();
        }
    }
}
