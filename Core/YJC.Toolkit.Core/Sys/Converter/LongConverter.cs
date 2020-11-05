using System;

namespace YJC.Toolkit.Sys.Converter
{
    class LongConverter : BaseTypeConverter<long>
    {
        public static readonly ITkTypeConverter Converter = new LongConverter();

        /// <summary>
        /// Initializes a new instance of the LongConverter class.
        /// </summary>
        private LongConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return Convert.ToInt64(text, ObjectUtil.SysCulture);
        }
    }
}
