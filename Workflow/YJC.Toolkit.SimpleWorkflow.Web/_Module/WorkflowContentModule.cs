using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Web;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowContentModule : IModule
    {
        private readonly ContentXml fConfig;
        private readonly bool fIsHistory;

        public WorkflowContentModule(ContentXml config, bool isHistory)
        {
            fConfig = config;
            fIsHistory = isHistory;
        }

        #region IModule 成员

        public string Title
        {
            get
            {
                return string.Empty;
            }
        }

        public IMetaData CreateMetaData(IPageData pageData)
        {
            ProcessDisplay display = GetDisplay(pageData);
            ContentXml normalConfig = new NormalContentXml(fConfig, display, fIsHistory);
            return new WorkflowMetaData(pageData, normalConfig.TableList);
        }

        public ISource CreateSource(IPageData pageData)
        {
            ProcessDisplay display = GetDisplay(pageData);
            return new WfContentSource(fConfig, display, fIsHistory);
        }

        public IPostObjectCreator CreatePostCreator(IPageData pageData)
        {
            throw new NotSupportedException();
        }

        public IPageMaker CreatePageMaker(IPageData pageData)
        {
            if (IsShowSource(true, pageData))
                return XmlPageMaker.DEFAULT;
            if (IsShowMetaData(true, pageData))
                return CreatePageMaker("<tk:MetaDataPageMaker />", pageData);
            //return XmlPageMaker.DEFAULT;
            string template = fIsHistory ? "WfHisContent" : "WfContent";
            RazorPageTemplatePageMaker pageMaker = new RazorPageTemplatePageMaker(template, pageData);
            if (fConfig.PageMaker != null)
            {
                WfPageMakerConfig pmConfig = fConfig.PageMaker;
                if (!string.IsNullOrEmpty(pmConfig.RazorFile))
                    pageMaker.RazorFile = pmConfig.RazorFile;
                if (pmConfig.PageData != null)
                    pageMaker.PageData = pmConfig.PageData.CreateObject();
                if (pmConfig.Scripts != null && pmConfig.Scripts.Count > 0)
                    pageMaker.Scripts = new UserScript(pmConfig.Scripts);
            }
            return pageMaker;
        }

        public IRedirector CreateRedirector(IPageData pageData)
        {
            throw new NotSupportedException();
        }

        public bool IsSupportLogOn(IPageData pageData)
        {
            return true;
        }

        public bool IsDisableInjectCheck(IPageData pageData)
        {
            return false;
        }

        public bool IsCheckSubmit(IPageData pageData)
        {
            return false;
        }

        #endregion IModule 成员

        private static ProcessDisplay GetDisplay(IPageData pageData)
        {
            var display = pageData.QueryString["Display"].Value<ProcessDisplay>(
                ProcessDisplay.Normal);
            return display;
        }

        public static IPageMaker CreatePageMaker(string xml, IPageData pageData)
        {
            return xml.ReadXmlFromFactory<IConfigCreator<IPageMaker>>(
                PageMakerConfigFactory.REG_NAME).CreateObject(pageData);
        }

        public static bool IsShowSource(bool showSource, IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return showSource && setting.IsDebug && !pageData.IsPost
                && setting.IsShowSource(pageData.QueryString);
        }

        public static bool IsShowMetaData(bool showSource, IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return showSource && setting.IsDebug && !pageData.IsPost
                && setting.IsShowMetaData(pageData.QueryString);
        }
    }
}