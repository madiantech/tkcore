using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public abstract class BaseMessage
    {
        protected BaseMessage()
        {
        }

        [NameModel(WeConst.XML_MODE, NamingRule = NamingRule.Pascal)]
        [SimpleElement(Order = 40, NamingRule = NamingRule.Lower, UseCData = true)]
        [TkTypeConverter(typeof(EnumFieldValueTypeConverter), UseObjectType = true)]
        public MessageType MsgType { get; protected set; }

        public string ToXml()
        {
            string result = this.WriteXml(WeConst.WRITE_SETTINGS, WeConst.ROOT);
            return result;
        }

        public string ToJson()
        {
            string result = this.WriteJson(WeConst.WRITE_SETTINGS);
            return result;
        }
    }
}