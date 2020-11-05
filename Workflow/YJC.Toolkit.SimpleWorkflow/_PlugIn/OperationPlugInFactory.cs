using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    public class OperationPlugInFactory : BasePlugInFactory
    {
        public const string REG_NAME = "_tk_Operation";
        private const string DESCRIPTION = "工作流非UI操作的插件工厂";

        public OperationPlugInFactory()
            : base(REG_NAME, DESCRIPTION)
        {
        }
    }
}