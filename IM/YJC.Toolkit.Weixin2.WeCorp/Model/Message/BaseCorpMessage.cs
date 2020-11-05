using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeCorp.Model.Message
{
    public abstract class BaseCorpMessage : BaseMessage
    {
        private List<string> fToUser;
        private List<string> fToParty;
        private List<string> fToTag;

        protected BaseCorpMessage(MessageType msgType)
        {
            MsgType = msgType;
        }

        protected BaseCorpMessage(int agentId, MessageType msgType)
            : this(msgType)
        {
            AgentId = agentId;
        }

        protected BaseCorpMessage(string appName, MessageType msgType)
            : this(msgType)
        {
            WeCorpSettings settings = WeCorpConfiguration.Create(null);
            AgentId = settings.GetAgentId(appName);
        }

        protected bool NoUser => ToUser?.Count == 0 && ToParty?.Count == 0 && ToTag?.Count == 0;

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 10)]//成员ID列表 IsMultiple = true
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> ToUser
        {
            get
            {
                if (fToUser == null)
                    fToUser = new List<string>();
                return fToUser;
            }
            set => fToUser = value;
        }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 20)]//部门ID列表
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> ToParty
        {
            get
            {
                if (fToParty == null)
                    fToParty = new List<string>();
                return fToParty;
            }
            set => fToParty = value;
        }

        [SimpleElement(NamingRule = NamingRule.Lower, Order = 30)]//标签ID列表
        [TkTypeConverter(typeof(StringListTypeConverter))]
        public List<string> ToTag
        {
            get
            {
                if (fToTag == null)
                    fToTag = new List<string>();
                return fToTag;
            }
            set => fToTag = value;
        }

        [SimpleElement(NamingRule = NamingRule.Lower, UseSourceType = true, Order = 40)]//企业应用ID
        public int AgentId { get; set; }
    }
}