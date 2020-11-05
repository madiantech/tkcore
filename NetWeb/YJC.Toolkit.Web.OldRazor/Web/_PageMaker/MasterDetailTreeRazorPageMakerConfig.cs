using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [PageMakerConfig(Description = "主从表新建，修改，删除等综合的TreeRazorPageMaker，新建修改等操作为普通界面",
        NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2015-09-21")]
    internal class MasterDetailTreeRazorPageMakerConfig : BaseTreeRazorPageMakerConfig
    {
        public MasterDetailTreeRazorPageMakerConfig()
        {
            EditTemplateName = "NormalMultiEdit";
            DetailTemplateName = "MultiTreeDetail";
        }

        protected override IPageMaker CreatePostEditPageMaker(OverrideItemConfig config, PageStyle style)
        {
            return new TreePostPageMaker(ContentDataType.Json, PageStyle.List, null);
        }

        protected override RazorPageMaker CreateEditPageMaker(OverrideItemConfig config, IPageData pageData)
        {
            SetDefaultConfig(ref config, pageData);

            return base.CreateEditPageMaker(config, pageData);
        }

        protected override IPageMaker CreateCustomPageMaker(OverrideItemConfig config,
            IPageData pageData, PageStyleClass style)
        {
            if (MetaDataUtil.StartsWith(pageData.Style, "DetailList"))
            {
                return InternalWebRazorUtil.CreateDetailListPageMaker(config, pageData);
            }
            return base.CreateCustomPageMaker(config, pageData, style);
        }

        internal static void SetDefaultConfig(ref OverrideItemConfig config, IPageData pageData)
        {
            if (config != null && config.RetUrl != null)
                return;

            RetUrlConfig retUrl = new RetUrlConfig(PageStyle.Custom, true,
                new MarcoConfigItem(false, false, string.Format(ObjectUtil.SysCulture, "~/{0}.c",
                    pageData.SourceInfo.CcSource)));
            if (config == null)
            {
                config = new OverrideItemConfig
                {
                    RetUrl = retUrl
                };
            }
            else
                config.RetUrl = retUrl;
        }
    }
}