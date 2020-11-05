using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "Organization", Author = "YJC",
        CreateDate = "2018-04-21", Description = "步骤用户是指定组织下的所有用户")]
    internal class OrganizationStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return new OrganizationStepUser(OrgCode);
        }

        #endregion IConfigCreator<IManualStepUser> 成员

        [SimpleAttribute(Required = true)]
        public string OrgCode { get; internal set; }
    }
}