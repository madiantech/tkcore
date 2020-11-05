using System;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Rule
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public abstract class RuleAttribute : BasePlugInAttribute
    {
        public static readonly RuleAttribute Empty = EmptyRuleAttribute.EmptyRule;

        protected RuleAttribute(MessageType msgType)
        {
            MsgType = msgType;
            Suffix = "Rule";
        }

        public MessageType MsgType { get; private set; }

        public int AppId { get; set; }

        public abstract bool Match(ReceiveMessage message);

        public override string FactoryName
        {
            get
            {
                return RulePlugInFactory.REG_NAME;
            }
        }
    }
}