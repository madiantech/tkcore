using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Weixin.Message
{
    public class EncodeReplyMessage
    {
        internal EncodeReplyMessage(string encrypt, string msgSignature, string timeStamp, string nonce)
        {
            Encrypt = encrypt;
            MsgSignature = msgSignature;
            TimeStamp = timeStamp;
            Nonce = nonce;
        }

        [SimpleElement(UseCData = true, Order = 10)]
        public string Encrypt { get; private set; }

        [SimpleElement(UseCData = true, Order = 20)]
        public string MsgSignature { get; private set; }

        [SimpleElement(Order = 30)]
        public string TimeStamp { get; private set; }

        [SimpleElement(UseCData = true, Order = 40)]
        public string Nonce { get; private set; }

        public string ToXml()
        {
            string xml = this.WriteXml(WeConst.WRITE_SETTINGS, WeConst.ROOT);
            return xml;
        }
    }
}
