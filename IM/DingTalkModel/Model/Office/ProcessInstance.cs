using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Office
{
    internal class ProcessInstance
    {
        public ProcessInstance()
        {
        }

        public ProcessInstance(string processInstanceId)
        {
            this.ProcessInstanceId = processInstanceId;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string ProcessInstanceId { get; set; }
    }
}