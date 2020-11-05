using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model
{
    internal class StringListTypeConverter : BaseSplitTypeConverter<List<string>>
    {
        public StringListTypeConverter()
            : base('|')
        {
        }

        protected override List<string> Convert(string[] data)
        {
            return new List<string>(data);
        }
    }
}