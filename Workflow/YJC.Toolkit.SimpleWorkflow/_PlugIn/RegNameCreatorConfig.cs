using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [CreatorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-29",
        Description = "通过注册名创建Creator")]
    internal class RegNameCreatorConfig : RegFactoryConfig<Creator>
    {
        public RegNameCreatorConfig()
            : base(CreatorPlugInFactory.REG_NAME)
        {
        }
    }
}