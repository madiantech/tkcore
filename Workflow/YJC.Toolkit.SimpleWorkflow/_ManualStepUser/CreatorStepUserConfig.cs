using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Creator", Author = "YJC",
        CreateDate = "2018-04-21", Description = "步骤用户是流程创建者")]
    internal class CreatorStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return CreatorStepUser.Instance;
        }

        #endregion IConfigCreator<IManualStepUser> 成员
    }
}