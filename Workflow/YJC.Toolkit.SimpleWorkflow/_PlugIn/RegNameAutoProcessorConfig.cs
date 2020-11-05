using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [AutoProcessorConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC", CreateDate = "2018-04-29",
        Description = "通过注册名创建AutoProcessor")]
    internal class RegNameAutoProcessorConfig : RegFactoryConfig<AutoProcessor>
    {
        public RegNameAutoProcessorConfig()
            : base(AutoProcessorPlugInFactory.REG_NAME)
        {
        }
    }
}