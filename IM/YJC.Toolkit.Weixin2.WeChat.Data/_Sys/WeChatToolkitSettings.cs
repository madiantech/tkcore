using System;
using System.Collections.Generic;
using System.IO;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeChat.Model.Message;
using YJC.Toolkit.WeChat.Rule;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.WeChat
{
    internal sealed class WeChatToolkitSettings
    {
        private static WeChatToolkitSettings fCurrent;
        private readonly MessageEngine fEngine;
        //private readonly IMessageLog fLog;
        //private readonly DefaultEngine fNormalDefault;

        internal WeChatToolkitSettings(BaseAppSetting appsetting)
        {
            fCurrent = this;

            fEngine = new MessageEngine();
            //if (xml.Weixin.Normal != null)
            //{
            //    fNormalDefault = new DefaultEngine(xml.Weixin.Normal.DefaultMessage);

            //    fAuthConfig = new WeixinAuthConfig();
            //    string authFileName = Path.Combine(appsetting.XmlPath, @"Weixin\Auth.xml");
            //    if (File.Exists(authFileName))
            //        fAuthConfig.ReadXmlFromFile(authFileName);
            //}
            //else
            //    TkDebug.ThrowImpossibleCode(this);

            //if (xml.Weixin.MessageLog != null)
            //    fLog = xml.Weixin.MessageLog.CreateObject();
        }

        internal MessageEngine Engine
        {
            get
            {
                return fEngine;
            }
        }

        public static WeChatToolkitSettings Current
        {
            get
            {
                if (fCurrent == null)
                {
                    string path = Path.Combine(BaseAppSetting.Current.XmlPath, "weixin.xml");
                    TkDebug.ThrowToolkitException(string.Format(ObjectUtil.SysCulture,
                        "{0}没有配置，请检查确认", path), null);
                }
                return fCurrent;
            }
            internal set
            {
                fCurrent = value;
            }
        }

        //internal string GetStateUrl(string state)
        //{
        //    WeixinSettings.Current.AssertNormalMode();
        //    return fAuthConfig.GetStateUrl(state);
        //}

        //private void ExecuteLog(ReceiveMessage message)
        //{
        //    fLog.Log(message);
        //}

        public BaseSendMessage NormalReply(ReceiveMessage message)
        {
            BaseSendMessage result;
            if (Engine.Reply(message, out result))
                return result;
            //if (WeixinSettings.Current.EnableService)
            //{
            //return new ServiceSendMessage(message);
            //}
            //return fNormalDefault.Reply(message);
            return null;
        }

        //internal void Log(ReceiveMessage message)
        //{
        //    if (fLog != null)
        //    {
        //        TkDebug.ThrowIfNoGlobalVariable();
        //        TkDebug.ThrowIfNoAppSetting();

        //        if (BaseAppSetting.Current.UseWorkThread)
        //            BaseGlobalVariable.Current.BeginInvoke(new Action<ReceiveMessage>(ExecuteLog), message);
        //        else
        //            ExecuteLog(message);
        //    }
        //}
    }
}