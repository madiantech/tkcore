using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ProcessorConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-03-29", Author = "YJC",
        Description = "根据配置的注册名实例化相应的Processor")]
    internal class RegNameProcessorConfig : RegFactoryConfig<Processor>
    {
        public RegNameProcessorConfig()
            : base(ProcessorPlugInFactory.REG_NAME)
        {
        }
    }
}