using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class DataRightConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_DataRight";
        internal const string DESCRIPTION = "数据权限的配置插件工厂";

        public DataRightConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
