using System;

namespace YJC.Toolkit.Sys
{
    public abstract class BaseDateTimeFormatTypeConverter : ITkTypeConverter, IConvertStatus
    {
        private readonly string fFormat;

        protected BaseDateTimeFormatTypeConverter(string format)
        {
            fFormat = format;
        }

        #region ITkTypeConverter 成员

        public string DefaultValue
        {
            get
            {
                return FormatDateTime(DateTime.MinValue);
            }
        }

        public object ConvertFromString(string text, ReadSettings settings)
        {
            IsSuccess = true;
            try
            {
                return DateTime.ParseExact(text, fFormat, ObjectUtil.SysCulture);
            }
            catch
            {
                IsSuccess = false;
                return DateTime.MinValue;
            }
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            try
            {
                DateTime date = (DateTime)value;
                return FormatDateTime(date);
            }
            catch
            {
                return DefaultValue;
            }
        }

        #endregion

        #region IConvertStatus 成员

        public bool IsSuccess { get; private set; }

        #endregion

        private string FormatDateTime(DateTime date)
        {
            return date.ToString(fFormat, ObjectUtil.SysCulture);
        }
    }
}
