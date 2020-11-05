using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class ColumnRightConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ColumnRight";
        internal const string DESCRIPTION = "列权限的配置插件工厂";

        public ColumnRightConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
