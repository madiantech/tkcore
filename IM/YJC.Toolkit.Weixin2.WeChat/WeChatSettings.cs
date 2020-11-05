using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat
{
    public class WeChatSettings
    {
        protected WeChatSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeChatSettings class.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="token"></param>
        public WeChatSettings(string appId, string appSecret, string token, string openId)
        {
            TkDebug.AssertArgumentNullOrEmpty(appId, "openId", null);
            TkDebug.AssertArgumentNullOrEmpty(appSecret, "secret", null);
            TkDebug.AssertArgumentNullOrEmpty(token, "token", null);
            TkDebug.AssertArgumentNullOrEmpty(openId, "openId", null);

            AppId = appId;
            AppSecret = appSecret;
            Token = token;
            OpenId = openId;
            MessageMode = MessageMode.Normal;
        }

        public WeChatSettings(string appId, string appSecret, string token, string openId,
            MessageMode messageMode, string encodingAESKey)
            : this(appId, appSecret, token, openId)
        {
            TkDebug.AssertArgumentNull(messageMode != MessageMode.Normal && string.IsNullOrEmpty(encodingAESKey),
                "encodingAESKey", "在混合和加密模式下，必须配置EncodingAESKey");

            MessageMode = messageMode;
            EncodingAESKey = encodingAESKey;
        }

        public string AppId { get; protected set; }

        public string AppSecret { get; protected set; }

        public string Token { get; protected set; }

        public string OpenId { get; protected set; }

        public MessageMode MessageMode { get; protected set; }

        public string EncodingAESKey { get; protected set; }

        public bool LogRawMessage { get; set; }
    }
}