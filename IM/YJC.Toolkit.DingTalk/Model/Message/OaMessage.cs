using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk.Model.Message
{
    public class OaMessage : BaseMessage
    {
        public OaMessage(OaConfig oa)
            : base(MessageType.OA)
        {
            TkDebug.AssertArgumentNull(oa, "oa", null);

            Oa = oa;
        }

        [ObjectElement(NamingRule = NamingRule.Camel)]
        public OaConfig Oa { get; set; }
    }
}