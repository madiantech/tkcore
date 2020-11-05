using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class FieldValueProviderNameConverter : BaseTypeConverter<FieldValueProviderName>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            return new FieldValueProviderName(text);
        }
    }
}