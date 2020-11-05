using System.IO;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat.Web
{
    internal class WeixinPostObjectCreator : IPostObjectCreator
    {
        public static readonly IPostObjectCreator Creator = new WeixinPostObjectCreator();

        private WeixinPostObjectCreator()
        {
        }

        #region IPostObjectCreator 成员

        public object Read(IInputData input, Stream stream)
        {
            ReceiveMessage message;
            string encodeType = input.QueryString[QueryStringConst.QS_ENCODE_TYPE].Value<string>(
                QueryStringConst.NORMAL_MSG);
            string tenantId = input.QueryString[WeChatUtil.TENANT_ID];
            WeChatSettings settings = WeChatConfiguration.Create(tenantId);
            if (settings.LogRawMessage)
            {
                using (MemoryStream dataStream = new MemoryStream())
                {
                    FileUtil.CopyStream(stream, dataStream);
                    try
                    {
                        WeChatWebUtil.WriteRawFile(input, dataStream);
                    }
                    catch { }

                    dataStream.Position = 0;
                    message = ReadMessage(input, dataStream, encodeType);
                }
            }
            else
                message = ReadMessage(input, stream, encodeType);

            return message;
        }

        #endregion IPostObjectCreator 成员

        private static ReceiveMessage ReadMessage(IInputData input, Stream stream, string encodeType)
        {
            ReceiveMessage message;
            if (encodeType == QueryStringConst.NORMAL_MSG)
            {
                message = new ReceiveMessage();
                message.ReadFromStream("Xml", WeConst.XML_MODE, stream, ObjectUtil.ReadSettings, WeConst.ROOT);
            }
            else
            {
                EncodeReceiveMessage encodeMsg = new EncodeReceiveMessage();
                encodeMsg.ReadFromStream("Xml", null, stream, ObjectUtil.ReadSettings, WeConst.ROOT);
                message = encodeMsg.CreateReceiveMessage(
                    input.QueryString[WeChatUtil.TENANT_ID],
                    input.QueryString[QueryStringConst.QS_MSG_SIGNATURE],
                    input.QueryString[QueryStringConst.QS_TIMESTAMP],
                    input.QueryString[QueryStringConst.QS_NONCE]);
            }
            return message;
        }
    }
}