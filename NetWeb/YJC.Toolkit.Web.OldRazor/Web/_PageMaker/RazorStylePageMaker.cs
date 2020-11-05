using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class RazorStylePageMaker : IPageMaker, ISupportMetaData, ICallerInfo
    {
        private readonly RazorStylePageMakerConfig fConfig;
        private IMetaData fMetaData;
        private readonly WebPageInfo fPageInfo;

        public RazorStylePageMaker(RazorStylePageMakerConfig config, IPageData pageData)
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
            ITemplateSuite suite = DefaultRazorTemplateSuite.Default;
            var template = suite.GetStyleTemplate(fConfig.TemplateStyle, true);

            string content = CreateRazorContent(template, pageData, WebRazorUtil.GetModel(outputData),
                WebRazorUtil.GetViewBag(pageData, fMetaData, new UserScript(fConfig.Scripts),
                fConfig.RazorData, MergeList(fConfig.Assemblies, template.Assemblies)));
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

        private static List<string> MergeList(List<string> list1, IEnumerable<string> list2)
        {
            if (list1 == null && list2 == null)
                return null;
            if (list2 == null)
                return list1;
            if (list1 == null)
                return list2.ToList();
            return list1.Union(list2).ToList();
        }

        protected virtual string CreateRazorContent(RazorSuiteItem item, IPageData pageData, 
            object model, DynamicObjectBag viewBag)
        {
            return RazorTemplateUtil.Execute(item, fConfig.RazorFile, model, viewBag);
        }
    }
}
