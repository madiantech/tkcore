using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat
{
    public static class WeChatExtension
    {
        private static ReceiveMessage CreateReceiveMessage(string xml)
        {
            ReceiveMessage result = new ReceiveMessage();
            result.ReadXml(xml, ReadSettings.Default, WeConst.ROOT);
            return result;
        }

        public static ReceiveMessage CreateReceiveMessage(this EncodeReceiveMessage msg,
            string tenantId, string msgSignature, string timeStamp, string nonce)
        {
            WeChatSettings settings = WeChatConfiguration.Create(tenantId);
            var xml = WeUtil.DecryptMsg(msg, settings.Token, settings.EncodingAESKey,
                msgSignature, timeStamp, nonce);
            return CreateReceiveMessage(xml);
        }
    }
}