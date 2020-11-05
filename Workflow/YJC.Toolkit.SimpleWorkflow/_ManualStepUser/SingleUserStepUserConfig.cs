using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "SingleUser", Author = "YJC",
        CreateDate = "2018-04-21", Description = "步骤用户是指定的用户Id")]
    internal class SingleUserStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return new SingleUserStepUser(UserId);
        }

        #endregion IConfigCreator<IManualStepUser> 成员

        [SimpleAttribute(Required = true)]
        public string UserId { get; internal set; }
    }
}