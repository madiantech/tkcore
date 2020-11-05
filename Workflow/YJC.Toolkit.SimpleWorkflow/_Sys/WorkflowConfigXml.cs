using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class WorkflowConfigXml : ToolkitConfig
    {
        [DynamicElement(UserManagerConfigFactory.REG_NAME)]
        [TagElement(NamespaceType = NamespaceType.Toolkit, Required = true)]
        public IConfigCreator<IUserManager> UserManager { get; private set; }
    }
}