namespace YJC.Toolkit.Sys
{
    public class NullableConverter : ITkTypeConverter
    {
        private readonly ITkTypeConverter fConverter;

        public NullableConverter(ITkTypeConverter converter)
        {
            TkDebug.AssertArgumentNull(converter, "converter", null);

            fConverter = converter;
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
            if (string.IsNullOrEmpty(text))
                return null;

            object result = fConverter.ConvertFromString(text, settings);
            IConvertStatus status = fConverter as IConvertStatus;
            if (status != null && !status.IsSuccess)
                return null;
            return result;
        }

        public string ConvertToString(object value, WriteSettings settings)
        {
            if (value == null)
                return null;
            return fConverter.ConvertToString(value, settings);
        }

        #endregion
    }
}
