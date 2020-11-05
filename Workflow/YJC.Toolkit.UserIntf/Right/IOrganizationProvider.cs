using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IOrganizationProvider : ITree
    {
        /// <summary>
        /// 获取组织
        /// </summary>
        /// <param name="orgCode">组织代码</param>
        /// <returns>组织</returns>
        IOrganization GetOrganization(string orgCode);

        /// <summary>
        /// 获取父组织
        /// </summary>
        /// <param name="orgCode">组织代码</param>
        /// <returns>父组织</returns>
        IOrganization GetParentOrganization(string orgCode);

        IOrganization GetParentDeparentment(string orgCode);

        IOrganization GetGrandParentOrganization(string orgCode);

        IOrganization GetGrandParentDeparentment(string orgCode);

        /// <summary>
        /// 获取指定用户所属于的组织的列表
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <returns>组织列表</returns>
        IEnumerable<IOrganization> GetOrganizationsForUser(string logOnName);

        /// <summary>
        /// 获取指定用户所属于的主组织
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <returns>组织</returns>
        IOrganization GetMainOrganizationsForUser(string logOnName);

        /// <summary>
        /// 获取属于指定组织的用户的列表
        /// </summary>
        /// <param name="orgCode">组织代码</param>
        /// <returns>用户列表</returns>
        IEnumerable<IUser> GetUsersInOrganization(string orgCode);

        /// <summary>
        /// 获取属于指定主组织的用户
        /// </summary>
        /// <param name="orgCode">组织代码</param>
        /// <returns>用户</returns>
        IUser GetUserInMainOrganization(string orgCode);

        /// <summary>
        /// 获取属于指定组织和指定角色的用户的列表
        /// </summary>
        /// <param name="orgCode">组织代码</param>
        /// <param name="roleCode">角色代码</param>
        /// <returns>用户列表</returns>
        IEnumerable<IUser> FindUsersByRole(string orgCode, string roleCode);

        /// <summary>
        /// 获取属于指定主组织和指定角色的用户
        /// </summary>
        /// <param name="orgCode">组织代码</param>
        /// <param name="roleCode">角色代码</param>
        /// <returns>用户</returns>
        IUser FindUserInMainOrganization(string orgCode, string roleCode);

        /// <summary>
        /// 指示指定用户是否属于指定组织
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <param name="orgCode">组织代码</param>
        /// <returns>如果指定用户属于指定组织，则为 true；否则为 false</returns>
        bool IsUserInOrganization(string logOnName, string orgCode);

        /// <summary>
        /// 应用程序的名称
        /// </summary>
        string ApplicationName { get; }
    }
}