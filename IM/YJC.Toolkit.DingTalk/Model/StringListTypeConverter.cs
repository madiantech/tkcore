using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class StringListTypeConverter : BaseSplitTypeConverter<List<string>>
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