using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class ObjectOperatorCollectionTypeConverter : BaseTypeConverter<ObjectOperatorCollection>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            string[] arr = text.Split('|');
            var result = (from item in arr
                          where !string.IsNullOrEmpty(item)
                          select item).Distinct();
            return new ObjectOperatorCollection(result);
        }
    }
}