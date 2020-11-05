using System.Globalization;

namespace YJC.Toolkit.Sys.Converter
{
    class CultureInfoConverter : BaseTypeConverter<CultureInfo>
    {
        public static ITkTypeConverter Converter = new CultureInfoConverter();

        private CultureInfoConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return new CultureInfo(text);
        }

        public override string ConvertToString(object value, WriteSettings settings)
        {
            CultureInfo info = value as CultureInfo;
            if (info != null)
                return info.Name;
            return null;
        }
    }
}
