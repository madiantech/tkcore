using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    internal class CommaStringListTypeConverter : BaseSplitTypeConverter<List<string>>
    {
        public CommaStringListTypeConverter()
            : base(',')
        {
        }

        protected override List<string> Convert(string[] data)
        {
            return new List<string>(data);
        }
    }
}