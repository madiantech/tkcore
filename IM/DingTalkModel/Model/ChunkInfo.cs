using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model
{
    public class ChunkInfo
    {
        public ChunkInfo(string agentId, string uploadId, int chunkSequence)
        {
            AgentId = agentId;
            UploadId = uploadId;
            ChunkSequence = chunkSequence;
        }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string AgentId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public string UploadId { get; set; }

        [SimpleElement(NamingRule = NamingRule.UnderLineLower)]
        public int ChunkSequence { get; set; }
    }
}