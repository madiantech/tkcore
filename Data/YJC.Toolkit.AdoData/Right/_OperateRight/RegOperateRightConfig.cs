using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [OperateRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-12-15", Description = "根据配置的注册名实例化操作权限")]
    internal class RegOperateRightConfig : RegFactoryConfig<IOperateRight>
    {
        public RegOperateRightConfig()
            : base(OperateRightPlugInFactory.REG_NAME)
        {
        }
    }
}
