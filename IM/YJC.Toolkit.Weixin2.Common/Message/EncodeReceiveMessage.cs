using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class EncodeReceiveMessage
    {
        [SimpleElement]
        public string ToUserName { get; protected set; }

        [SimpleElement]
        public string Encrypt { get; protected set; }

        //protected static ReceiveMessage CreateReceiveMessage(string xml)
        //{
        //    ReceiveMessage result = new ReceiveMessage();
        //    result.ReadXml(xml, ReadSettings.Default, WeConst.ROOT);
        //    return result;
        //}

        //public ReceiveMessage CreateReceiveMessage(string msgSignature,
        //    string timeStamp, string nonce)
        //{
        //    var xml = WeUtil.DecryptMsg(this, msgSignature, timeStamp, nonce);
        //    return CreateReceiveMessage(xml);
        //}
    }
}