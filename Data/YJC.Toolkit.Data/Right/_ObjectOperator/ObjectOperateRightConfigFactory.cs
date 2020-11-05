using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    public sealed class ObjectOperateRightConfigFactory : BaseXmlConfigFactory
    {
        public const string REG_NAME = "_tk_xml_ObjectOperateRight";
        internal const string DESCRIPTION = "基于对象的操作权限的配置插件工厂";

        public ObjectOperateRightConfigFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}
