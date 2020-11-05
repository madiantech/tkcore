using System;
using System.IO;
using YJC.Toolkit.Sys;
using YJC.Toolkit.WeChat.Rule;

namespace YJC.Toolkit.WeChat
{
    internal class WeChatToolkitInitialization : IInitialization
    {
        #region IInitialization 成员

        public void AppStarting(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
        }

        public void AppStarted(object application, BaseAppSetting appsetting,
            BaseGlobalVariable globalVariable)
        {
            //string path = Path.Combine(appsetting.XmlPath, "weixin.xml");
            //if (File.Exists(path))
            //{
            //    WeixinExtraXml xml = new WeixinExtraXml();
            //    xml.ReadXmlFromFile(path);
            //    WeixinToolkitSettings.Current = new WeixinToolkitSettings(xml, appsetting);
            //}

            TkDebug.ThrowIfNoAppSetting();

            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, "wechat.xml");
            if (File.Exists(fileName))
            {
                WeChatXml xml = new WeChatXml();
                xml.ReadXmlFromFile(fileName);
                WeChatSettingCreator creator = new WeChatSettingCreator(xml);
                WeChatConfiguration.SetCreator(creator);
            }

            WeChatToolkitSettings.Current = new WeChatToolkitSettings(appsetting);

            RulePlugInFactory factory = globalVariable.FactoryManager.GetCodeFactory(
                RulePlugInFactory.REG_NAME).Convert<RulePlugInFactory>();

            factory.EnumableCodePlugIn(AddCodeRule);
            factory.EnumableXmlPlugIn(AddXmlRule);
        }

        public void AppEnd(object application)
        {
        }

        #endregion IInitialization 成员

        private void AddCodeRule(string regName, Type regType, BasePlugInAttribute attribute)
        {
            RuleAttribute attr = attribute.Convert<RuleAttribute>();
            if (string.IsNullOrEmpty(attr.RegName))
                attr.RegName = attr.GetRegName(regType);
            WeChatToolkitSettings.Current.Engine.Add(attr);
        }

        private void AddXmlRule(IXmlPlugInItem config, Type xmlType, BasePlugInAttribute dummyAttribute)
        {
            RuleConfigItem item = config.Convert<RuleConfigItem>();
            RuleAttribute attr = item.Match.CreateObject();
            attr.RegName = config.RegName;
            //attr.AppId = item.AppId;
            WeChatToolkitSettings.Current.Engine.Add(attr);
        }
    }
}