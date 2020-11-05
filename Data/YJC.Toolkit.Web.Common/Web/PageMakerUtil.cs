using System;
using System.Data;
using System.Linq;
using System.Xml;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Xml;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Web
{
    public static class PageMakerUtil
    {
        public static readonly IPageMaker XmlPageMaker = "<tk:XmlPageMaker/>".CreateFromXmlFactory<IPageMaker>(
            PageMakerConfigFactory.REG_NAME);

        private static string GetTypeString(SourceOutputType[] types)
        {
            return string.Join(", ", types);
        }

        internal static void AssertType(object source, OutputData data,
            params SourceOutputType[] types)
        {
            bool found = Array.Exists(types, type => type == data.OutputType);
            TkDebug.Assert(found, string.Format(ObjectUtil.SysCulture,
                "当前的PageMaker只支持{0}数据类型，而传入的WebOutputData的数据类型为{1}，无法兼容",
                GetTypeString(types), data.OutputType), source);
        }

        public static string GetString(OutputData data)
        {
            string result = data.Data.Convert<string>();
            return result;
        }

        public static XmlReader GetXmlReader(OutputData data)
        {
            XmlReader result = data.Data.Convert<XmlReader>();
            return result;
        }

        public static T GetObject<T>(OutputData data) where T : class
        {
            T result = data.Data.Convert<T>();
            return result;
        }

        public static Func<ISource, IInputData, OutputData, bool> IsType(SourceOutputType type)
        {
            return (source, pageData, outputData) => outputData.OutputType == type;
        }

        public static Func<ISource, IInputData, OutputData, bool> IsType(params SourceOutputType[] types)
        {
            return (source, pageData, outputData) => types.Any(type => outputData.OutputType == type);
        }

        public static bool IsShowSource(bool showSource, IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return showSource && setting.IsDebug && !pageData.IsPost
                && setting.IsShowSource(pageData.QueryString);
        }

        public static bool IsShowJson(bool showSource, IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return showSource && setting.IsDebug && !pageData.IsPost
                && setting.IsShowJson(pageData.QueryString);
        }

        public static bool IsShowExcel(bool showSource, IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return (pageData.Style.Style == PageStyle.List || MetaDataUtil.StartsWith(pageData.Style, "DetailList"))
                && showSource && setting.IsShowExcel(pageData.QueryString);
        }

        public static bool IsShowMetaData(bool showSource, IPageData pageData)
        {
            TkDebug.ThrowIfNoAppSetting();

            WebAppSetting setting = WebAppSetting.WebCurrent;
            return showSource && setting.IsDebug && !pageData.IsPost
                && setting.IsShowMetaData(pageData.QueryString);
        }

        public static XmlReader GetDataSetReader(OutputData outputData)
        {
            XmlReader reader = null;
            switch (outputData.OutputType)
            {
                case SourceOutputType.XmlReader:
                    reader = outputData.Data.Convert<XmlReader>();
                    break;

                case SourceOutputType.DataSet:
                    reader = new XmlDataSetReader(outputData.Data.Convert<DataSet>());
                    break;
            }
            return reader;
        }
    }
}