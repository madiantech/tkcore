using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    [DataRightConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2013-10-22", Description = "根据配置的注册名实例化数据权限")]
    internal class RegDataRightConfig : RegFactoryConfig<IDataRight>
    {
        public RegDataRightConfig()
            : base(DataRightPlugInFactory.REG_NAME)
        {
        }
    }
}
