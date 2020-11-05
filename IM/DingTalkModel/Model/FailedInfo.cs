using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    //获取回调失败的结果
    public class FailedInfo
    {
        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<CorpResult<CallBackData>> UserAddOrg { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<CorpResult<BpmsCallBackData>> BpmsInstanceChange { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<CorpResult<BpmsCallBackData>> BpmsTaskChange { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<CorpResult<RoleLabelChange>> LabelConfAdd { get; set; }

        //[SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        //[TkTypeConverter(typeof(IMDateTimeConverter))]
        //public DateTime EventTime { get; set; }

        //[SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        //public string CallBackTag { get; set; }

        //[SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        //public List<string> UserId { get; set; }

        //[SimpleElement(NamingRule = NamingRule.Lower)]
        //public string CorpId { get; set; }

        //[SimpleElement(NamingRule = NamingRule.Lower, IsMultiple = true)]
        //public List<string> DeptId { get; set; }

        //[SimpleElement(NamingRule = NamingRule.Camel)]
        //public string CallbackData { get; set; }
    }
}