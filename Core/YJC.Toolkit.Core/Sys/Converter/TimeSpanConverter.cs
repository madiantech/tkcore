using System;

namespace YJC.Toolkit.Sys.Converter
{
    class TimeSpanConverter : BaseTypeConverter<TimeSpan>
    {
        public static readonly ITkTypeConverter Converter = new TimeSpanConverter();
        private TimeSpanConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return TimeSpan.Parse(text, ObjectUtil.SysCulture);
        }
    }
}
