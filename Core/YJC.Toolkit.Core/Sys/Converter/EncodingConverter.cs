using System;
using System.Text;

namespace YJC.Toolkit.Sys.Converter
{
    class EncodingConverter : BaseTypeConverter<Encoding>
    {
        public static readonly ITkTypeConverter Converter = new EncodingConverter();

        private EncodingConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            if (string.Compare(text, "utf-8", StringComparison.OrdinalIgnoreCase) == 0)
                return ToolkitConst.UTF8;
            return Encoding.GetEncoding(text);
        }

        public override string ConvertToString(object value, WriteSettings settings)
        {
            Encoding encoding = value as Encoding;
            if (encoding != null)
                return encoding.WebName;
            else
                return null;
        }
    }
}
