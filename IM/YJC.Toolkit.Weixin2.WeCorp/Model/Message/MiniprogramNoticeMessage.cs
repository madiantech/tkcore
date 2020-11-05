using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public class MiniProgramNoticeMessage : BaseCorpMessage
    {
        public MiniProgramNoticeMessage(MiniProgramNotice notice)
            : base(MessageType.MiniprogramNotice)
        {
            TkDebug.AssertArgumentNull(notice, nameof(notice), null);

            Notice = notice;
        }

        [ObjectElement(LocalName = "miniprogram_notice", Order = 50)]
        public MiniProgramNotice Notice { get; set; }
    }
}