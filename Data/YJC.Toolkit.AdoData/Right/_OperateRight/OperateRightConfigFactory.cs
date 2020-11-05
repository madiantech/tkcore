using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class OperateRightConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_OperateRight";
        internal const string DESCRIPTION = "操作权限的配置插件工厂";

        public OperateRightConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
