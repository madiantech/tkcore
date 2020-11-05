using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Log;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class Tk5ModuleConfigItem
    {
        [SimpleAttribute(DefaultValue = true)]
        public bool ShowSource { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, Required = true)]
        public MultiLanguageText Title { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(MetaDataConfigFactory.REG_NAME)]
        public IConfigCreator<IMetaData> MetaData { get; private set; }

        [TagElement(NamespaceType.Toolkit, Required = true)]
        [DynamicElement(SourceConfigFactory.REG_NAME)]
        public IConfigCreator<ISource> Source { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(PostObjectConfigFactory.REG_NAME)]
        public IConfigCreator<IPostObjectCreator> PostObjectCreator { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(PageMakerConfigFactory.REG_NAME)]
        public IConfigCreator<IPageMaker> PageMaker { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [DynamicElement(RedirectorConfigFactory.REG_NAME)]
        public IConfigCreator<IRedirector> Redirector { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public LogConfigItem Log { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "Item")]
        public List<LogConditionConfigItem> Logs { get; private set; }

        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "RecordLog")]
        public List<RecordLogConfigItem> RecordLogs { get; private set; }

        [ObjectElement(NamespaceType.Toolkit)]
        public BaseWebPageConfigItem WebPage { get; private set; }

        [TagElement(NamespaceType.Toolkit)]
        [ObjectElement(NamespaceType.Toolkit, LocalName = "WebPage", IsMultiple = true)]
        public List<WebPageConfigItem> WebPages { get; private set; }

        private BaseWebPageConfigItem GetFitableConfig(IPageData pageData)
        {
            if (WebPage != null)
                return WebPage;

            if (WebPages != null)
            {
                var result = (from item in WebPages
                              where MetaDataUtil.Equals(item.Style, pageData.Style)
                              select item).FirstOrDefault();
                return result;
            }

            return null;
        }

        public LogConfigItem GetLog(ISource source, IInputData pageData, OutputData outputData)
        {
            if (Log != null)
                return Log;
            if (Logs != null)
            {
                foreach (var item in Logs)
                {
                    if (item.Condition.UseCondition(source, pageData, outputData))
                        return item.Log;
                }
            }
            return null;
        }

        public string GetTitle()
        {
            return Title == null ? string.Empty : Title.ToString();
        }

        public bool IsSupportLogon(IPageData pageData)
        {
            BaseWebPageConfigItem config = GetFitableConfig(pageData);
            if (config == null)
                return true;

            return config.SupportLogOn;
        }
    }
}