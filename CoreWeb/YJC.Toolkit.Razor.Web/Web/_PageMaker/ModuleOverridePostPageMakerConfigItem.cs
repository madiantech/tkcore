using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class ModuleOverridePostPageMakerConfigItem : BasePostPageMakerConfig
    {
        [SimpleAttribute]
        public PageStyleClass Style { get; private set; }
    }
}