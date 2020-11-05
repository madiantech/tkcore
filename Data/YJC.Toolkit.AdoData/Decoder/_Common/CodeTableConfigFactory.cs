using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    public class CodeTableConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_CodeTable";
        private const string DESCRIPTION = "代码表的配置插件工厂";

        public CodeTableConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
