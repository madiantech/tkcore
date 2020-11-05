using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class HashSetPageStyleTypeConverter : BaseTypeConverter<HashSet<PageStyleClass>>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return null;

                text = text.Replace(',', ' ');
                string[] data = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                HashSet<PageStyleClass> result = new HashSet<PageStyleClass>(
                    from item in data select item.Trim().Value<PageStyleClass>());
                return result;
            }
            catch
            {
                return null;
            }
        }

        public override string ConvertToString(object value, WriteSettings settings)
        {
            HashSet<PageStyleClass> data = value as HashSet<PageStyleClass>;
            if (data != null)
                return string.Join(" ", data.ToString());
            return null;
        }
    }
}
