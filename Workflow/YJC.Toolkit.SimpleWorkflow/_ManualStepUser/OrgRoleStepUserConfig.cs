using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    [ManualStepUserConfig(NamespaceType = NamespaceType.Toolkit, RegName = "OrgRole", Author = "YJC",
        CreateDate = "2018-04-21", Description = "步骤用户是指定组织和指定角色交叉得到用户")]
    internal class OrgRoleStepUserConfig : IConfigCreator<IManualStepUser>
    {
        #region IConfigCreator<IManualStepUser> 成员

        public IManualStepUser CreateObject(params object[] args)
        {
            return new OrgRoleStepUser(RoleCode, OrgCode);
        }

        #endregion IConfigCreator<IManualStepUser> 成员

        [SimpleAttribute(Required = true)]
        public string RoleCode { get; internal set; }

        [SimpleAttribute(Required = true)]
        public string OrgCode { get; internal set; }
    }
}