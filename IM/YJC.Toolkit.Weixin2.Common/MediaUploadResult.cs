using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin
{
    public class MediaUploadResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public FileType Type { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string MediaId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(IMDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return MediaId;
        }
    }
}