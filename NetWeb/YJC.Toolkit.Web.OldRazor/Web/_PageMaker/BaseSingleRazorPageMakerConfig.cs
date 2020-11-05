using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal abstract class BaseSingleRazorPageMakerConfig : BaseCompositeRazorPageMakerConfig
    {
        protected BaseSingleRazorPageMakerConfig()
        {
        }

        [SimpleAttribute(DefaultValue = TableDisplayType.Striped)]
        public TableDisplayType? ListDisplay { get; protected set; }

        [SimpleAttribute]
        public int? DialogHeight { get; protected set; }

        protected override IPageMaker CreateListPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            ListRazorPageMakerConfig listConfig = new ListRazorPageMakerConfig(ListTemplateName, config);
            if (listConfig.RazorData == null)
            {
                NormalListDataConfig data = new NormalListDataConfig();
                string xml = string.Format(ObjectUtil.SysCulture, "<Toolkit Display='{0}' DialogHeight='{1}' />",
                    ListDisplay, DialogHeight);
                data.ReadXml(xml, ReadSettings.Default, QName.ToolkitNoNS);
                listConfig.RazorData = data;
            }
            else
            {
                NormalListDataConfig data = listConfig.RazorData as NormalListDataConfig;
                if (data != null)
                {
                    if (ListDisplay.HasValue)
                        data.Display = ListDisplay.Value;
                    if (DialogHeight.HasValue)
                        data.DialogHeight = DialogHeight.Value;
                }
            }
            return new ListRazorPageMaker(listConfig, pageData);
        }

        protected override RazorPageMaker CreateDetailPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            RazorPageMakerConfig makerConfig = new RazorPageMakerConfig(DetailTemplateName, config);
            if (makerConfig.RazorData == null)
            {
                NormalDetailDataConfig data = new NormalDetailDataConfig();
                string xml = string.Format(ObjectUtil.SysCulture, "<Toolkit DialogHeight='{0}' />",
                    DialogHeight);
                data.ReadXml(xml, ReadSettings.Default, QName.ToolkitNoNS);
                makerConfig.RazorData = data;
            }
            else
            {
                NormalDetailDataConfig data = makerConfig.RazorData as NormalDetailDataConfig;
                if (data != null)
                {
                    if (DialogHeight.HasValue)
                        data.DialogHeight = DialogHeight.Value;
                }
            }
            return new RazorPageMaker(makerConfig, pageData);
        }
    }
}