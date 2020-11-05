using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    public class ProcessInstanceResult : BaseResult
    {
        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Title { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CreateTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string FinishTime { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string OriginatorUserid { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string OriginatorDeptId { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(UpperCaseEnumConverter), UseObjectType = true)]
        public ApprovalStatus Status { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ApproverUserids { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string CcUserids { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<ComponentsInfo> FormComponentValues { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        [TkTypeConverter(typeof(LowerCaseEnumConverter), UseObjectType = true)]
        public Result Result { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string BusinessId { get; set; }

        [ObjectElement(NamingRule = NamingRule.UnderLineLower, IsMultiple = true)]
        public List<OperationRecordsInfo> OperationRecords { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel, IsMultiple = true)]
        public List<TasksInfo> Tasks { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string OriginatorDeptName { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        [TkTypeConverter(typeof(UpperCaseEnumConverter), UseObjectType = true)]
        public BizAction BizAction { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string AttachedProcessInstanceIds { get; set; }
    }
}