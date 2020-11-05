using System;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
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