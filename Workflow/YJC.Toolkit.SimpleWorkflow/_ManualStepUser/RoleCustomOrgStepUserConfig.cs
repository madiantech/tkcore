using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "RoleCustomOrg", Author = "YJC",
        CreateDate = "2018-04-21", Description = "步骤用户是通过表达式计算得到组织Code和指定的角色Code交叉得到的用户")]
    internal class RoleCustomOrgStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return new RoleCustomOrgStepUser(RoleCode, Expression);
        }

        #endregion IConfigCreator<IManualStepUser> 成员

        [SimpleAttribute(Required = true)]
        public string RoleCode { get; internal set; }

        [SimpleAttribute(Required = true)]
        public string Expression { get; internal set; }
    }
}