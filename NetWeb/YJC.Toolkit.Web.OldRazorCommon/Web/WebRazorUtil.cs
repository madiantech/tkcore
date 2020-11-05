using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal static class WebRazorUtil
    {
        private const string DIALOG_DATA = "<tk:NormalEditData DialogMode=\"true\" ShowTitle=\"false\" />";
        private const string DETALLIST_DATA = "<tk:NormalListData OperatorPosition=\"None\"/>";

        internal static NormalListDataConfig CreateDefaultDetailListData()
        {
            return DETALLIST_DATA.ReadXmlFromFactory<NormalListDataConfig>(RazorDataConfigFactory.REG_NAME);
        }

        public static NormalEditDataConfig CreateDefaultDialogData()
        {
            return DIALOG_DATA.ReadXmlFromFactory<NormalEditDataConfig>(RazorDataConfigFactory.REG_NAME);
        }

        public static object GetModel(OutputData outputData)
        {
            switch (outputData.OutputType)
            {
                case SourceOutputType.String:
                case SourceOutputType.ToolkitObject:
                case SourceOutputType.DataSet:
                case SourceOutputType.Object:
                    return outputData.Data;

                case SourceOutputType.XmlReader:
                    return XDocument.Load(outputData.Data.Convert<XmlReader>());

                case SourceOutputType.FileContent:
                    TkDebug.ThrowToolkitException("不支持FileContent格式", null);
                    return null;

                default:
                    TkDebug.ThrowImpossibleCode(null);
                    return null;
            }
        }

        public static DynamicObjectBag GetViewBag(IPageData page, IMetaData metaData,
            UserScript script, IConfigCreator<object> pageData, List<string> assemblies)
        {
            DynamicObjectBag viewBag = new DynamicObjectBag();
            viewBag.AddValue("Title", page.Title);
            if (metaData != null)
            {
                viewBag.AddValue("MetaData", metaData);
                string title = metaData.Title;
                if (!string.IsNullOrEmpty(title))
                    viewBag.SetValue("Title", title);
            }
            if (script != null)
                viewBag.AddValue("Script", script);
            IEnumerable<string> pdAssemblies = null;
            if (pageData != null)
            {
                object pd = pageData.CreateObject();
                if (pd != null)
                {
                    viewBag.AddValue("PageData", pd);
                    pdAssemblies = RazorUtil.GetAdditionAssemblies(pd.GetType());
                }
            }
            if (pdAssemblies != null)
            {
                if (assemblies == null)
                    assemblies = new List<string>(pdAssemblies);
                else
                    assemblies.AddRange(pdAssemblies);
            }
            if (assemblies != null)
                viewBag.AddValue("Assembly", assemblies);

            return viewBag;
        }

        public static DynamicObjectBag GetNewViewBag(IPageData page, IMetaData metaData,
            UserScript script, object pageData, IEnumerable<string> assemblies)
        {
            DynamicObjectBag viewBag = new DynamicObjectBag();
            viewBag.AddValue("Title", page.Title);
            if (metaData != null)
            {
                viewBag.AddValue("MetaData", metaData);
                string title = metaData.Title;
                if (!string.IsNullOrEmpty(title))
                    viewBag.SetValue("Title", title);
            }
            if (script != null)
                viewBag.AddValue("Script", script);
            if (pageData != null)
                viewBag.AddValue("PageData", pageData);
            if (assemblies != null)
                viewBag.AddValue("Assembly", assemblies);

            return viewBag;
        }
    }
}