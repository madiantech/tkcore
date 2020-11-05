using System.Data;
using System.Text;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class RazorPageMaker : IPageMaker, ISupportMetaData, ICallerInfo
    {
        private readonly RazorPageMakerConfig fConfig;
        private IMetaData fMetaData;
        private readonly WebPageInfo fPageInfo;

        public RazorPageMaker(RazorPageMakerConfig config, IPageData pageData)
        {
            fConfig = config;
            fPageInfo = new WebPageInfo(pageData, WebGlobalVariable.SessionGbl,
                RetUrlConfig.GetRetUrl(config.RetUrl, pageData), pageData.PageUrl);
        }

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            return true;
        }

        public void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            fMetaData = metaData;
        }

        #endregion

        #region IPageMaker 成员

        public IContent WritePage(ISource source, IPageData pageData, OutputData outputData)
        {
            string content = CreateRazorContent(pageData, WebRazorUtil.GetModel(outputData),
                WebRazorUtil.GetViewBag(pageData, fMetaData, new UserScript(fConfig.Scripts),
                fConfig.RazorData, fConfig.Assemblies));
            return new SimpleContent(ContentTypeConst.HTML, content);
        }

        #endregion

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

        #endregion

        public IConfigCreator<object> RazorData
        {
            get
            {
                return fConfig.RazorData;
            }
            set
            {
                fConfig.RazorData = value;
            }
        }

        protected virtual string CreateRazorContent(IPageData pageData, object model,
            DynamicObjectBag viewBag)
        {
            return RazorTemplateUtil.Execute(fConfig.Template, fConfig.RazorFile, model, viewBag);
        }
    }
}
