using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [ObjectContext]
    [UpdatedActionConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2017-05-02",
        Author = "YJC", Description = "根据配置的注册名实例化UpdatedAction")]
    internal class RegUpdatedActionConfig : RegFactoryConfig<BaseUpdatedAction>
    {
        public RegUpdatedActionConfig()
            : base(UpdatedActionPlugInFactory.REG_NAME)
        {
        }
    }
}