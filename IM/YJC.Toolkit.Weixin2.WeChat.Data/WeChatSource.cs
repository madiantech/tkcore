using YJC.Toolkit.Sys;
using YJC.Toolkit.WeChat.Model.Message;
using YJC.Toolkit.Weixin;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat
{
    public sealed class WeChatSource : ISource
    {
        public static readonly ISource Source = new WeChatSource();

        private WeChatSource()
        {
        }

        #region ISource 成员

        public OutputData DoAction(IInputData input)
        {
            if (!input.IsPost)
            {
                string tenantId = input.QueryString[WeChatUtil.TENANT_ID];
                WeChatSettings settings = WeChatConfiguration.Create(tenantId);
                if (WeUtil.CheckSignature(settings.Token,
                    input.QueryString[QueryStringConst.QS_SIGNATURE],
                    input.QueryString[QueryStringConst.QS_TIMESTAMP],
                    input.QueryString[QueryStringConst.QS_NONCE]))
                {
                    return OutputData.Create(input.QueryString[QueryStringConst.QS_ECHO_STR]);
                }
                return OutputData.Create("验名错误");
            }
            else
            {
                ReceiveMessage message = input.PostObject.Convert<ReceiveMessage>();
                BaseSendMessage result = WeChatToolkitSettings.Current.NormalReply(message);
                //WeixinToolkitSettings.Current.Log(message);

                if (result != null)
                {
                    string encodeType = input.QueryString[QueryStringConst.QS_ENCODE_TYPE]
                        .Value<string>(QueryStringConst.NORMAL_MSG);
                    if (encodeType == QueryStringConst.NORMAL_MSG)
                        return OutputData.CreateToolkitObject(result);
                    else
                    {
                        WeChatSettings settings = WeChatConfiguration.CreateWithOpenId(message.ToUserName);
                        EncodeReplyMessage reply = WeUtil.EncryptMsg(settings.AppId, settings.Token,
                            settings.EncodingAESKey, result.ToXml(),
                            input.QueryString[QueryStringConst.QS_TIMESTAMP],
                            input.QueryString[QueryStringConst.QS_NONCE]);
                        return OutputData.CreateToolkitObject(reply);
                    }
                }
                else
                    return OutputData.Create(string.Empty);
            }
        }

        #endregion ISource 成员
    }
}