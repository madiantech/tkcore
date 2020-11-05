namespace YJC.Toolkit.Sys
{
    internal sealed class QNameConverter : BaseTypeConverter<QName>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            return QName.GetFullName(text);
        }
    }
}
