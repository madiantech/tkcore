using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp.Model.ExternalContacts
{
    internal class ExtContactParam
    {
        public ExtContactParam(string externalUserId, string handoverUserId, string takeoverUserId)
        {
            ExternalUserId = externalUserId;
            HandoverUserId = handoverUserId;
            TakeoverUserId = takeoverUserId;
        }

        [SimpleElement(LocalName = "external_userid")]
        public string ExternalUserId { get; set; }

        [SimpleElement(LocalName = "handover_userid")]
        public string HandoverUserId { get; set; }

        [SimpleElement(LocalName = "takeover_userid")]
        public string TakeoverUserId { get; set; }
    }
}