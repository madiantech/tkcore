using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class ListSearchConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ListSearch";
        private const string DESCRIPTION = "ListSearch配置插件工厂";

        public ListSearchConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
