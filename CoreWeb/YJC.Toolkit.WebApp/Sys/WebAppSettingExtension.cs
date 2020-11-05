using System;
using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    internal static class WebAppSettingExtension
    {
        public static void Config(this WebAppSetting setting, WebDefaultXmlConfig extXml)
        {
            WebConfigItem defaultConfig = extXml?.WebConfig;
            setting.DefaultPageMaker = defaultConfig?.DefaultPageMaker ?? new SourceOutputPageMakerConfig();
            setting.DefaultRedirector = defaultConfig?.DefaultRedirector ?? new OutputRedirectorConfig();
            setting.DefaultPostCreator = defaultConfig?.DefaultPostObjectCreator ?? new JsonPostDataSetCreatorConfig();
            setting.ReadSettings = defaultConfig?.ReadSettings ?? ReadSettings.Default;
            setting.WriteSettings = defaultConfig?.WriteSettings ?? WriteSettings.Default;

            //ExceptionHandlerConfigItem exConfig = extXml.ExceptionHandler;
            //if (exConfig != null)
            //{
            //    //ErrorPageHandler = exConfig.ErrorPageException;
            //    //ErrorOpeartionHandler = exConfig.ErrorOperationException;
            //    //ReLogOnHandler = exConfig.ReLogonException;
            //    //ToolkitHandler = exConfig.ToolkitException;
            //    //ExceptionHandler = exConfig.Exception;
            //}

            //var tempHandleConfig = new PageMakerExceptionHandlerConfig()
            //{
            //    PageMaker = new TempPageMakerConfig(ExceptionPageMaker.Instance)
            //};
            //if (ErrorPageHandler == null)
            //    ErrorPageHandler = tempHandleConfig;
            //if (ErrorOpeartionHandler == null)
            //{
            //    ErrorOpeartionHandler = tempHandleConfig;
            //}
            //if (ReLogOnHandler == null)
            //    ReLogOnHandler = new ReLogonExceptionHandlerConfig();
            //if (ToolkitHandler == null)
            //    ToolkitHandler = tempHandleConfig;
            //if (ExceptionHandler == null)
            //    ExceptionHandler = new PageMakerExceptionHandlerConfig()
            //    {
            //        Log = true,
            //        PageMaker = new TempPageMakerConfig(ExceptionPageMaker.Instance)
            //    };
        }

        public static void SetPath(this WebAppSetting appSetting, WebAppXml xml)
        {
            string resolveStartPath = AppUtil.ResolveUrl(appSetting.StartupPath);
            appSetting.LogOnPath = AppUtil.ResolveUrl(xml.Application.Url.LogOnPath);
            if (string.IsNullOrEmpty(appSetting.LogOnPath))
                appSetting.LogOnPath = resolveStartPath;
            appSetting.HomePath = AppUtil.ResolveUrl(xml.Application.Url.HomePath);
            if (string.IsNullOrEmpty(appSetting.HomePath))
                appSetting.HomePath = resolveStartPath;
            appSetting.MainPath = AppUtil.ResolveUrl(xml.Application.Url.MainPath);

            if (xml.Upload != null)
            {
                //if (!string.IsNullOrEmpty(fUploadVirtualPath))
                //    fUploadVirtualPath = VirtualPathUtility.AppendTrailingSlash(fUploadVirtualPath);
                //if (!string.IsNullOrEmpty(fUploadTempVirtualPath))
                //    fUploadTempVirtualPath = VirtualPathUtility.AppendTrailingSlash(fUploadTempVirtualPath);
            }
            // 假处理，以后需要考虑解决方案
            appSetting.AppVirtualPath = "/";
            //AppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            //if (string.IsNullOrEmpty(AppVirtualPath))
            //    AppVirtualPath = "/";
            //AppVirtualPath = VirtualPathUtility.AppendTrailingSlash(AppVirtualPath);
        }
    }
}