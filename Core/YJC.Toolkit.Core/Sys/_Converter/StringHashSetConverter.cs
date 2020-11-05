using System;
using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.Sys
{
    public class StringHashSetConverter : BaseTypeConverter<HashSet<string>>
    {
        protected override object InternalConvertFromString(string text, ReadSettings settings)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return null;

                text = text.Replace(',', ' ');
                string[] data = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                HashSet<string> result = new HashSet<string>(from item in data select item.Trim());
                return result;
            }
            catch
            {
                return null;
            }
        }

        public override string ConvertToString(object value, WriteSettings settings)
        {
            HashSet<string> data = value as HashSet<string>;
            if (data != null)
                return string.Join(" ", data);
            return null;
        }
    }
}
