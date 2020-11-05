using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class ObjectOperatorsConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ObjectOperators";
        internal const string DESCRIPTION = "基于对象的操作权限和操作符配置的配置插件工厂";

        public ObjectOperatorsConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
