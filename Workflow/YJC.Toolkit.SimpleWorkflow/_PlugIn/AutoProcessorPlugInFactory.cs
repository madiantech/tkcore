using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class AutoProcessorPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_Workflow_AutoProcessor";
        private const string DESCRIPTION = "AutoProcessor插件工厂";

        public AutoProcessorPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}