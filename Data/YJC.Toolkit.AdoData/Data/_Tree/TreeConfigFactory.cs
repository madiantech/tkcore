using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TreeConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Tree";
        private const string DESCRIPTION = "生成Tree结构的配置插件工厂";

        public TreeConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
