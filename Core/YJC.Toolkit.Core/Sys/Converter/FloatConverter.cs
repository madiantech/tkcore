using System;

namespace YJC.Toolkit.Sys.Converter
{
    class FloatConverter : BaseTypeConverter<float>
    {
        public static readonly ITkTypeConverter Converter = new FloatConverter();

        /// <summary>
        /// Initializes a new instance of the FloatConverter class.
        /// </summary>
        private FloatConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return Convert.ToSingle(text, ObjectUtil.SysCulture);
        }
    }
}
