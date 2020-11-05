using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data.Constraint
{
    public class ConstraintConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_Constraint";
        internal const string DESCRIPTION = "字段校验的配置插件工厂";

        public ConstraintConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
