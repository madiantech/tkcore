using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class OperatorConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_OperateOperator";
        internal const string DESCRIPTION = "操作符的配置插件工厂";

        public OperatorConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
