using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    internal class TkxXml : ToolkitConfig
    {
        [SimpleElement(NamespaceType.Toolkit)]
        public string PageHandler { get; private set; }
    }
}