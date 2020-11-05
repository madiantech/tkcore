using System;

namespace YJC.Toolkit.Sys.Converter
{
    class GuidConverter : BaseTypeConverter<Guid>
    {
        public static readonly ITkTypeConverter Converter = new GuidConverter();

        /// <summary>
        /// Initializes a new instance of the GuidConverter class.
        /// </summary>
        private GuidConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return Guid.Parse(text);
        }
    }
}
