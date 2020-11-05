using System;

namespace YJC.Toolkit.Sys.Converter
{
    class DateTimeConverter : BaseTypeConverter<DateTime>
    {
        public static readonly ITkTypeConverter Converter = new DateTimeConverter();

        private DateTimeConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            if (string.IsNullOrEmpty(text))
                return default(DateTime);
            if (text.Length == settings.DateTimeFormat.Length)
                return DateTime.ParseExact(text, settings.DateTimeFormat, ObjectUtil.SysCulture);

            return DateTime.Parse(text, ObjectUtil.SysCulture);
        }

        public override string ConvertToString(object value, WriteSettings settings)
        {
            if (value == null)
                return null;
            try
            {
                DateTime date = (DateTime)value;
                return date.ToString(settings.DateTimeFormat, ObjectUtil.SysCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}
