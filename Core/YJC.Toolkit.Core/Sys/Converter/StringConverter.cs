
namespace YJC.Toolkit.Sys.Converter
{
    class StringConverter : BaseTypeConverter<string>
    {
        public static readonly ITkTypeConverter Converter = new StringConverter();

        /// <summary>
        /// Initializes a new instance of the StringConverter class.
        /// </summary>
        private StringConverter()
        {
        }

        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return text;
        }
    }
}
