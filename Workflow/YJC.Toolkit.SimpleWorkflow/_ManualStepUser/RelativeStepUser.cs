using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Right;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.SimpleWorkflow
{
    internal class RelativeStepUser : IManualStepUser
    {
        private static IPostProvider fPostProvider;
        private static IUserProvider fUserProvider;
        private static IOrganizationProvider fOrgProvider;

        private readonly string fRelativeStepName;
        private readonly RelativeUserType fRelativeUserType;
        private readonly RelationType fRelationType;
        private readonly string fRoleCode;

        public RelativeStepUser(string relativeStepName, RelativeUserType relativeUserType,
            RelationType relationType, string roleCode)
        {
            TkDebug.AssertArgumentNullOrEmpty(relativeStepName, "relativeStepName", null);
            TkDebug.AssertArgumentNullOrEmpty(roleCode, "roleCode", null);

            fRelativeStepName = relativeStepName;
            fRelativeUserType = relativeUserType;
            fRelationType = relationType;
            fRoleCode = roleCode;
        }

        #region IManualStepUser 成员

        public IEnumerable<string> GetUserList(WorkflowContent content,
            DataRow workflowRow, IDbDataSource source)
        {
            string relativeUser = GetUserByStepName(fRelativeStepName, workflowRow, source);
            return GetUsersByRelative(relativeUser);
        }

        #endregion IManualStepUser 成员

        private static IOrganizationProvider OrgProvider
        {
            get
            {
                if (fOrgProvider == null)
                {
                    fOrgProvider = WorkflowSetting.Current.UserManager.OrgProvider;
                }
                return fOrgProvider;
            }
        }

        private static IPostProvider PostProvider
        {
            get
            {
                if (fPostProvider == null)
                {
                    fPostProvider = WorkflowSetting.Current.UserManager.PostProvider;
                }
                return fPostProvider;
            }
        }

        private static IUserProvider UserProvider
        {
            get
            {
                if (fUserProvider == null)
                {
                    fUserProvider = WorkflowSetting.Current.UserManager.UserProvider;
                }
                return fUserProvider;
            }
        }

        private string GetUserByStepName(string relativeName, DataRow workflowRow, IDbDataSource source)
        {
            StepInstResolver stepResolver = new StepInstResolver(source);
            using (stepResolver)
            {
                DataRow lastStepRow = stepResolver.SelectRowWithParams(new string[] { "WiId", "CurrentStep" },
                    workflowRow["Id"], relativeName);
                string relativeUser = string.Empty;
                switch (fRelativeUserType)
                {
                    case RelativeUserType.Processor:
                        relativeUser = lastStepRow["ProcessId"].ToString();
                        break;

                    case RelativeUserType.Sender:
                        relativeUser = lastStepRow["SendId"].ToString();
                        break;

                    case RelativeUserType.Receiver:
                        relativeUser = lastStepRow["ReceiveId"].ToString();
                        break;
                }
                return relativeUser;
            }
        }

        private IEnumerable<string> GetUsersByRelative(string userId)
        {
            switch (fRelationType)
            {
                case RelationType.GrandParentDept:
                    string logOnName = GetlogOnNameById(userId);
                    IOrganization dep = GetMainDepByUser(logOnName);
                    TkDebug.AssertNotNull(dep, string.Format(ObjectUtil.SysCulture,
                        "编号为 {0} ， 登录名为 {1} 的用户所属的部门不能为空", userId, logOnName), this);
                    IOrganization gpDep = OrgProvider.GetGrandParentDeparentment(dep.OrgCode);
                    TkDebug.AssertNotNull(gpDep, string.Format(ObjectUtil.SysCulture,
                        "部门 {0} 的上上级部门不能为空", gpDep.OrgCode), this);
                    return StepUserUtil.Convert(OrgProvider.FindUsersByRole(gpDep.OrgCode, fRoleCode));

                case RelationType.GrandParentOrg:
                    logOnName = GetlogOnNameById(userId);
                    IOrganization org = OrgProvider.GetMainOrganizationsForUser(logOnName);
                    TkDebug.AssertNotNull(org, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户所属的组织不能为空", logOnName), this);
                    IOrganization gpOrg = OrgProvider.GetGrandParentOrganization(org.OrgCode);
                    TkDebug.AssertNotNull(gpOrg, string.Format(ObjectUtil.SysCulture,
                        "组织 {0} 的的上上级组织不能为空", org.OrgCode), this);
                    return StepUserUtil.Convert(OrgProvider.FindUsersByRole(gpOrg.OrgCode, fRoleCode));

                case RelationType.GrandParentPost:
                    logOnName = GetlogOnNameById(userId);
                    IPost post = PostProvider.GetMainPostsForUser(logOnName);
                    TkDebug.AssertNotNull(post, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户的岗位不能为空", logOnName), this);
                    IPost gfPost = PostProvider.GetGrandParentPost(post.PostCode);
                    TkDebug.AssertNotNull(post, string.Format(ObjectUtil.SysCulture,
                        "岗位 {0} 的上上级岗位不能为空", post.PostCode), this);
                    return StepUserUtil.Convert(PostProvider.FindPostUsersByRole(gfPost.PostCode, fRoleCode));

                case RelationType.ParentDept:
                    logOnName = GetlogOnNameById(userId);
                    dep = GetMainDepByUser(logOnName);
                    TkDebug.AssertNotNull(dep, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户所属部门不能为空", logOnName), this);
                    IOrganization pDep = OrgProvider.GetParentDeparentment(dep.OrgCode);
                    TkDebug.AssertNotNull(pDep, string.Format(ObjectUtil.SysCulture,
                        "部门 {0} 的上级部门不能为空", dep.OrgCode), this);
                    return StepUserUtil.Convert(OrgProvider.FindUsersByRole(pDep.OrgCode, fRoleCode));

                case RelationType.ParentOrg:
                    logOnName = GetlogOnNameById(userId);
                    dep = OrgProvider.GetMainOrganizationsForUser(logOnName);
                    TkDebug.AssertNotNull(dep, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户所属组织不能为空", logOnName), this);
                    pDep = OrgProvider.GetParentOrganization(dep.OrgCode);
                    TkDebug.AssertNotNull(pDep, string.Format(ObjectUtil.SysCulture,
                        "组织 {0} 的上级组织不能为空", dep.OrgCode), this);
                    return StepUserUtil.Convert(OrgProvider.FindUsersByRole(pDep.OrgCode, fRoleCode));

                case RelationType.ParentPost:
                    logOnName = GetlogOnNameById(userId);
                    post = PostProvider.GetMainPostsForUser(logOnName);
                    TkDebug.AssertNotNull(post, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户所属岗位不能为空", logOnName), this);
                    IPost pPost = PostProvider.GetParentPost(post.PostCode);
                    TkDebug.AssertNotNull(post, string.Format(ObjectUtil.SysCulture,
                        "岗位 {0} 的上级岗位不能为空", logOnName), this);
                    return StepUserUtil.Convert(PostProvider.FindPostUsersByRole(pPost.PostCode, fRoleCode));

                case RelationType.SameDept:
                    logOnName = GetlogOnNameById(userId);
                    dep = GetMainDepByUser(logOnName);
                    TkDebug.AssertNotNull(dep, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户所属部门不能为空", logOnName), this);
                    return StepUserUtil.Convert(OrgProvider.FindUsersByRole(dep.OrgCode, fRoleCode));

                case RelationType.SameOrg:
                    logOnName = GetlogOnNameById(userId);
                    org = OrgProvider.GetMainOrganizationsForUser(logOnName);
                    TkDebug.AssertNotNull(org, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户所属组织不能为空", logOnName), this);
                    return StepUserUtil.Convert(OrgProvider.FindUsersByRole(org.OrgCode, fRoleCode));

                case RelationType.SamePerson:
                    return EnumUtil.Convert(userId);

                case RelationType.SamePost:
                    logOnName = GetlogOnNameById(userId);
                    post = PostProvider.GetMainPostsForUser(logOnName);
                    TkDebug.AssertNotNull(post, string.Format(ObjectUtil.SysCulture,
                        "登录名为 {0} 的用户所属岗位不能为空", logOnName), this);
                    return StepUserUtil.Convert(PostProvider.FindPostUsersByRole(post.PostCode, fRoleCode));

                default:
                    return EnumUtil.Convert(userId);
            }
        }

        private static string GetlogOnNameById(string id)
        {
            return UserProvider.GetUser(id).LogOnName;
        }

        private static IOrganization GetMainDepByUser(string logOnName)
        {
            return OrgProvider.GetOrganizationsForUser(logOnName).Single(
                a => a.IsDepartment == true);
        }
    }
}