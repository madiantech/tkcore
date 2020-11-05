using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public sealed class SourceConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Source";
        private const string DESCRIPTION = "数据源的配置插件工厂";

        public SourceConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
