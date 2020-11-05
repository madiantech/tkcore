using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeChat
{
    internal class WeChatSettingCreator : BaseXmlPlatformSettingCreator<WeChatSettings>,
        IWeChatPlatformSettingCreator
    {
        private readonly WeChatSettings fSettings;

        public WeChatSettingCreator(WeChatXml chatXml)
            : base(GetXmlFileName())
        {
            var config = chatXml.WeChat;
            fSettings = new WeChatSettings(config.AppId, config.AppSecret, config.Token,
                config.OpenId, config.MessageMode, config.EncodingAESKey)
            {
                LogRawMessage = config.LogRawMessage
            };
        }

        public override WeChatSettings Create(string tenantId)
        {
            return fSettings;
        }

        private static string GetXmlFileName()
        {
            TkDebug.ThrowIfNoAppSetting();

            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, "wechat_token.xml");
            return fileName;
        }

        #region IWeChatPlatformSettingCreator 成员

        public WeChatSettings CreateWithOpenId(string openId)
        {
            return fSettings;
        }

        #endregion IWeChatPlatformSettingCreator 成员
    }
}