using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class PicsInfo
    {
        [SimpleElement(Order = 10)] public int Count { get; private set; }

        [ObjectElement(IsMultiple = true, LocalName = "item", Order = 20)]
        public List<PicItem> Items { get; private set; }
    }
}