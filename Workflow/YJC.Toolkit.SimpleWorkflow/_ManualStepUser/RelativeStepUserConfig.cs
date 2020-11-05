using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Relative", Author = "YJC",
        CreateDate = "2018-04-21", Description = "步骤用户是相对另外一个步骤的用户关系得到有关的用户")]
    internal class RelativeStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return new RelativeStepUser(RelativeStepName, RelativeUserType, RelationType, RoleCode);
        }

        #endregion IConfigCreator<IManualStepUser> 成员

        [SimpleAttribute(Required = true)]
        public string RelativeStepName { get; internal set; }

        [SimpleAttribute(Required = true)]
        public RelativeUserType RelativeUserType { get; internal set; }

        [SimpleAttribute(Required = true)]
        public RelationType RelationType { get; internal set; }

        [SimpleAttribute]
        public string RoleCode { get; internal set; }
    }
}