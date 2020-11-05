using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ModuleOverridePageTemplateConfigItem : BaseRazorPageTemplatePageMakerConfig
    {
        [SimpleAttribute]
        public PageStyleClass Style { get; private set; }
    }
}