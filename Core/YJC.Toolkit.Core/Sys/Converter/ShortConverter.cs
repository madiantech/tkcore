using System;

namespace YJC.Toolkit.Sys.Converter
{
    class ShortConverter : BaseTypeConverter<short>
    {
        public static readonly ITkTypeConverter Converter = new ShortConverter();

        /// <summary>
        /// Initializes a new instance of the ShortConverter class.
        /// </summary>
        private ShortConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return Convert.ToInt16(text, ObjectUtil.SysCulture);
        }
    }
}
