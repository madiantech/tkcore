using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    internal class FileDetail
    {
        public FileDetail(string agentId, int fileSize)
        {
            AgentId = agentId;
            FileSize = fileSize;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int FileSize { get; set; }
    }
}