using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class RazorPageTemplatePageMaker : IPageMaker, ISupportMetaData, ICallerInfo
    {
        private IMetaData fMetaData;
        private WebPageInfo fPageInfo;

        public RazorPageTemplatePageMaker(string pageTemplateName, IPageData pageData)
            : this(pageTemplateName, (RetUrlConfig)null, pageData)
        {
        }

        internal RazorPageTemplatePageMaker(string pageTemplateName, RetUrlConfig config, IPageData pageData)
        {
            TkDebug.AssertArgumentNullOrEmpty(pageTemplateName, "pageTemplateName", null);

            PageTemplateName = pageTemplateName;
            fPageInfo = new WebPageInfo(pageData, WebGlobalVariable.SessionGbl,
                RetUrlConfig.GetRetUrl(config, pageData), pageData.PageUrl);
        }

        internal RazorPageTemplatePageMaker(string pageTemplateName,
            BaseRazorPageTemplatePageMakerConfig config, IPageData pageData)
            : this(pageTemplateName, config.RetUrl, pageData)
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
            IPageTemplate template = CreatePageTemplate(PageTemplateName);
            if (Scripts == null)
                Scripts = new UserScript(null);
            object pageDataObj = PageData;
            if (pageDataObj == null)
                pageDataObj = template.GetDefaultPageData(source, pageData, outputData);
            if (pageDataObj != null)
            {
                OnSetPageData(new PageDataEventArgs(source, pageData, outputData, pageDataObj));
            }

            object model = WebRazorUtil.GetModel(outputData);

            var viewBag = WebRazorUtil.GetNewViewBag(pageData, fMetaData, Scripts, pageDataObj);
            string content = Execute(template, PageTemplateName, ModelCreator, RazorFile,
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
            SetPageData?.Invoke(this, e);
        }

        internal void SetConfig(BaseRazorPageTemplatePageMakerConfig config)
        {
            if (config != null)
            {
                RazorFile = config.RazorFile;
                if (config.RazorData != null)
                    PageData = config.RazorData.CreateObject();
                Scripts = new UserScript(config.Scripts);
                ModelCreator = config.ModelCreator;
            }
        }

        internal void ResetCallInfo(RetUrlConfig retUrl, IPageData pageData)
        {
            if (retUrl != null)
                fPageInfo = new WebPageInfo(pageData, WebGlobalVariable.SessionGbl,
                    RetUrlConfig.GetRetUrl(retUrl, pageData), pageData.PageUrl);
        }

        private static IPageTemplate CreatePageTemplate(string regName)
        {
            TkDebug.AssertArgumentNullOrEmpty(regName, "regName", null);

            return PlugInFactoryManager.CreateInstance<IPageTemplate>(PageTemplatePlugInFactory.REG_NAME, regName);
        }

        private static string Execute(object model, dynamic viewBag, PageTemplateInitData initData,
            string razorFile, string layout, IPageTemplate pageTemplate, ISource source,
            IInputData input, OutputData outputData)
        {
            string engineName = pageTemplate.GetEngineName(source, input, outputData);

            IRazorEngine engine = RazorEnginePlugInFactory.CreateRazorEngine(engineName);

            string content = Task.Run(async ()
                => await RazorExtension.CompileRenderWithLayoutAsync(engine, razorFile,
                layout, model, initData, viewBag)).GetAwaiter().GetResult();

            return content;
        }

        private static string Execute(IPageTemplate template, string pageTemplateName, string modelCreator,
            string localRazorFile, object model, dynamic viewBag, ISource source,
            IInputData input, OutputData outputData)
        {
            string templateFile = template.GetTemplateFile(source, input, outputData);
            PageTemplateInitData initData = new PageTemplateInitData(pageTemplateName, modelCreator);
            string razorFile;
            string layout;
            if (string.IsNullOrEmpty(localRazorFile))
            {
                razorFile = templateFile;
                layout = null;
            }
            else
            {
                razorFile = localRazorFile;
                layout = templateFile;
            }
            return Execute(model, viewBag, initData, razorFile, layout, template, source, input, outputData);
        }
    }
}