using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class ProcessorPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "WorkflowProcessor";
        private const string DESCRIPTION = "工作流操作对象的工厂";

        public ProcessorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}