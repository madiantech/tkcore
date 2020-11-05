using System.Dynamic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class WebRazorUtil
    {
        private const string DIALOG_DATA = "<tk:NormalEditData DialogMode=\"true\" ShowTitle=\"false\" />";
        private const string DETALLIST_DATA = "<tk:NormalListData OperatorPosition=\"None\"/>";
        private const string RAZOR_NAME = "RazorTemplate";
        private const string DEFAULT_RAZOR_TEMPLATE = "Normal";

        static WebRazorUtil()
        {
            TemplateName = DefaultUtil.GetSimpleValue(RAZOR_NAME, DEFAULT_RAZOR_TEMPLATE);
        }

        public static string TemplateName { get; }

        public static string GetTemplateFile(string fileName)
        {
            TkDebug.AssertArgumentNullOrEmpty(fileName, nameof(fileName), null);

            return $"^{Path.Combine(TemplateName, fileName)}";
        }

        internal static NormalListDataConfig CreateDefaultDetailListData()
        {
            return DETALLIST_DATA.ReadXmlFromFactory<NormalListDataConfig>(RazorDataConfigFactory.REG_NAME);
        }

        internal static NormalEditDataConfig CreateDefaultDialogData()
        {
            return DIALOG_DATA.ReadXmlFromFactory<NormalEditDataConfig>(RazorDataConfigFactory.REG_NAME);
        }

        internal static object GetModel(OutputData outputData)
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

        internal static dynamic GetViewBag(IPageData page, IMetaData metaData, UserScript script,
            IConfigCreator<object> pageData)
        {
            object pd = null;
            if (pageData != null)
                pd = pageData.CreateObject();
            return GetNewViewBag(page, metaData, script, pd);
        }

        internal static dynamic GetNewViewBag(IPageData page, IMetaData metaData,
            UserScript script, object pageData)
        {
            dynamic viewBag = new ExpandoObject();
            viewBag.Title = page.Title;
            if (metaData != null)
            {
                viewBag.MetaData = metaData;
                string title = metaData.Title;
                if (!string.IsNullOrEmpty(title))
                    viewBag.Title = title;
            }
            if (script != null)
                viewBag.Script = script;
            if (pageData != null)
                viewBag.PageData = pageData;

            return viewBag;
        }
    }
}