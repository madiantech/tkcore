using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    internal class WebPageConfigItem : BaseWebPageConfigItem
    {
        [SimpleAttribute(Required = true)]
        public PageStyleClass Style { get; private set; }
    }
}
