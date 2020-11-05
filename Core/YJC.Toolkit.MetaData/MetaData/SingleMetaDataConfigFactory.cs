using YJC.Toolkit.Sys;

namespace YJC.Toolkit.MetaData
{
    public class SingleMetaDataConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_SingleMetaData";
        private const string DESCRIPTION = "单表MetaData的配置插件工厂";

        public SingleMetaDataConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
