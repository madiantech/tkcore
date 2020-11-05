using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.Sys
{
    internal static class WebAppSettingExtension
    {
        public static void Config(this WebAppSetting setting, WebAppExtensionXml extXml)
        {
            WebConfigItem defaultConfig = extXml.DefaultConfig;
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
            string resolveStartPath = WebUtil.ResolveUrl(appSetting.StartupPath);
            appSetting.LogOnPath = WebUtil.ResolveUrl(xml.Application.Url.LogOnPath);
            if (string.IsNullOrEmpty(appSetting.LogOnPath))
                appSetting.LogOnPath = resolveStartPath;
            appSetting.HomePath = WebUtil.ResolveUrl(xml.Application.Url.HomePath);
            if (string.IsNullOrEmpty(appSetting.HomePath))
                appSetting.HomePath = resolveStartPath;
            appSetting.MainPath = WebUtil.ResolveUrl(xml.Application.Url.MainPath);

            if (xml.Upload != null)
            {
                if (!string.IsNullOrEmpty(appSetting.UploadVirtualPath))
                    appSetting.UploadVirtualPath =
                        VirtualPathUtility.AppendTrailingSlash(appSetting.UploadVirtualPath);
                if (!string.IsNullOrEmpty(appSetting.UploadTempVirtualPath))
                    appSetting.UploadTempVirtualPath =
                        VirtualPathUtility.AppendTrailingSlash(appSetting.UploadTempVirtualPath);
            }
            appSetting.AppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            if (string.IsNullOrEmpty(appSetting.AppVirtualPath))
                appSetting.AppVirtualPath = "/";
            appSetting.AppVirtualPath = VirtualPathUtility.AppendTrailingSlash(appSetting.AppVirtualPath);
        }
    }
}