using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Composite", Author = "YJC",
        CreateDate = "2018-04-22", Description = "步骤用户是组合其他基本的配置来决定")]
    internal class CompositeStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return new CompositeStepUser(StepUsers);
        }

        #endregion IConfigCreator<IManualStepUser> 成员

        [DynamicElement(ManualStepUserConfigFactory.REG_NAME, IsMultiple = true, Required = true)]
        public List<IConfigCreator<IManualStepUser>> StepUsers { get; private set; }
    }
}