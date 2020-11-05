using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class RazorPageTemplatePageMaker : IPageMaker, ISupportMetaData, ICallerInfo
    {
        private IMetaData fMetaData;
        private readonly WebPageInfo fPageInfo;

        public RazorPageTemplatePageMaker(string pageTemplateName, IPageData pageData)
        {
            TkDebug.AssertArgumentNullOrEmpty(pageTemplateName, "pageTemplateName", null);

            PageTemplateName = pageTemplateName;
            fPageInfo = new WebPageInfo(pageData, WebGlobalVariable.SessionGbl,
                RetUrlConfig.GetRetUrl(null, pageData), pageData.PageUrl);
        }

        internal RazorPageTemplatePageMaker(string pageTemplateName,
            BaseRazorPageTemplatePageMakerConfig config, IPageData pageData)
            : this(pageTemplateName, pageData)
        {
            SetConfig(config);
        }

        internal RazorPageTemplatePageMaker(RazorPageTemplatePageMakerConfig config, IPageData pageData)
            : this(config.PageTemplate, config, pageData)
        {
        }

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            IPageTemplate template = RazorPageTemplateUtil.CreatePageTemplate(PageTemplateName);
            var templateAssemblies = RazorUtil.GetAdditionAssemblies(template.GetType());
            if (Scripts == null)
                Scripts = new UserScript(null);
            object pageDataObj = PageData;
            if (pageDataObj == null)
                pageDataObj = template.GetDefaultPageData(source, pageData, outputData);
            IEnumerable<string> pageDataAssemblies = null;
            if (pageDataObj != null)
            {
                OnSetPageData(new PageDataEventArgs(source, pageData, outputData, pageDataObj));
                pageDataAssemblies = RazorUtil.GetAdditionAssemblies(pageDataObj.GetType());
            }

            var viewBag = WebRazorUtil.GetNewViewBag(pageData, fMetaData, Scripts, pageDataObj,
                EnumUtil.Convert(templateAssemblies, pageDataAssemblies, Assemblies));
            string content = RazorPageTemplateUtil.Execute(template, PageTemplateName, ModelCreator, RazorFile,
                WebRazorUtil.GetModel(outputData), viewBag, source, pageData, outputData);

            return new SimpleContent(ContentTypeConst.HTML, content);
        }

        #endregion IPageMaker 成员

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion ISupportMetaData 成员

        #region ICallerInfo 成员

        public void AddInfo(DataSet dataSet)
        {
            fPageInfo.AddToDataSet(dataSet);
        }

        public void AddInfo(StringBuilder builder)
        {
            fPageInfo.AddToStringBuilder(builder);
        }

        public void AddInfo(XElement element)
        {
            fPageInfo.AddToXElement(element);
        }

        public void AddInfo(dynamic data)
        {
            fPageInfo.AddToDynamic(data);
        }

        #endregion ICallerInfo 成员

        public string PageTemplateName { get; private set; }

        public string RazorFile { get; set; }

        public object PageData { get; set; }

        public IEnumerable<string> Assemblies { get; set; }

        public UserScript Scripts { get; set; }

        public string ModelCreator { get; set; }

        public event EventHandler<PageDataEventArgs> SetPageData;

        protected virtual void OnSetPageData(PageDataEventArgs e)
        {
            if (SetPageData != null)
                SetPageData(this, e);
        }

        internal void SetConfig(BaseRazorPageTemplatePageMakerConfig config)
        {
            if (config != null)
            {
                RazorFile = config.RazorFile;
                if (config.RazorData != null)
                    PageData = config.RazorData.CreateObject();
                Assemblies = config.Assemblies;
                Scripts = new UserScript(config.Scripts);
                ModelCreator = config.ModelCreator;
            }
        }
    }
}