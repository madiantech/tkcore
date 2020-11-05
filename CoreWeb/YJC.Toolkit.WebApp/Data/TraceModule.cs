using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [Module(Author = "YJC", CreateDate = "2015-10-20", Description = "显示系统所有插件信息")]
    internal class TraceModule : BaseCustomModule
    {
        private const string PAGEMAKER = "<FreeRazorPageMaker FileName=\"^Normal/Bin/TraceManager.cshtml\"/>";

        private const string FACTORYMAKER = "<FreeRazorPageMaker FileName=\"^Normal/Bin/FactoryDetail.cshtml\"/>";

        private const string CONFIGMAKER = "<FreeRazorPageMaker FileName=\"^Normal/Bin/ConfigDetail.cshtml\"/>";

        public TraceModule()
            : base("系统插件一览")
        {
        }

        public override ISource CreateSource(IPageData pageData)
        {
            return new PlugInSource();
        }

        public override IPageMaker CreatePageMaker(IPageData pageData)
        {
            if (MetaDataUtil.Equals(pageData.Style, (PageStyleClass)"Code"))
            {
                return FACTORYMAKER.CreateFromXmlFactory<IPageMaker>(
                    PageMakerConfigFactory.REG_NAME, pageData);
            }
            else if (MetaDataUtil.Equals(pageData.Style, (PageStyleClass)"Xml"))
            {
                return CONFIGMAKER.CreateFromXmlFactory<IPageMaker>(
                    PageMakerConfigFactory.REG_NAME, pageData);
            }
            else
                return PAGEMAKER.CreateFromXmlFactory<IPageMaker>(
                    PageMakerConfigFactory.REG_NAME, pageData);
        }

        public override bool IsSupportLogOn(IPageData pageData)
        {
            return false;
        }
    }
}