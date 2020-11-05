using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public sealed class MetaDataConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_MetaData";
        private const string DESCRIPTION = "MetaData的配置插件工厂";

        public MetaDataConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
