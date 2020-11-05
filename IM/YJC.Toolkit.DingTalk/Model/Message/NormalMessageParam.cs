using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    internal class NormalMessageParam
    {
        public NormalMessageParam()
        {
        }

        public NormalMessageParam(string sender, string cid, BaseMessage msg)
        {
            Sender = sender;
            Cid = cid;
            Msg = msg;
        }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Sender { get; set; }

        [SimpleElement(NamingRule = NamingRule.Camel)]
        public string Cid { get; set; }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public BaseMessage Msg { get; set; }
    }
}