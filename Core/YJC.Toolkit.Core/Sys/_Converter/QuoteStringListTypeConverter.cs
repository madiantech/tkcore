namespace YJC.Toolkit.Sys
{
    class QuoteStringListTypeConverter : BaseTypeConverter<QuoteStringList>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return QuoteStringList.FromString(text);
        }
    }
}
