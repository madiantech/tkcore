using System;
using System.Collections.Generic;
using System.Linq;

namespace YJC.Toolkit.Sys
{
    public class QueryStringValue
    {
        private readonly string fValue;
        private bool fSingleValue;
        private List<string> fValues;

        private QueryStringValue(string value)
        {
            fSingleValue = true;
            fValue = Uri.UnescapeDataString(value);
        }

        public void Add(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            value = Uri.UnescapeDataString(value);
            if (fSingleValue)
            {
                fSingleValue = false;
                fValues = new List<string> { fValue, value };
            }
            else
                fValues.Add(value);
        }

        public IEnumerable<string> Values
        {
            get
            {
                if (fSingleValue)
                    return fValue.Split(',');
                else
                {
                    var result = from item in fValues
                                 let itemValues = item.Split(',')
                                 from itemValue in itemValues
                                 select itemValue;
                    return result;
                }
            }
        }

        public override string ToString()
        {
            if (fSingleValue)
                return fValue;
            else
                return string.Join(",", fValues);
        }

        public static QueryStringValue Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return new QueryStringValue(value);
        }
    }
}