using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class OperatorsConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Operators";
        internal const string DESCRIPTION = "操作权限和操作符配置的配置插件工厂";

        public OperatorsConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
