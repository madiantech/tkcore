using System.Collections.Generic;

namespace YJC.Toolkit.Right
{
    public interface IRoleProvider
    {
        /// <summary>
        /// 将指定用户添加到指定角色中
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="users">用户</param>
        void AddUserToRole(IRole role, params IUser[] users);

        /// <summary>
        /// 将指定的一些用户添加到相应的角色中
        /// </summary>
        /// <param name="list">用户角色匹配列表，Key为角色，Value为用户</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        void AddUsersToRoles(IDictionary<IRole, IEnumerable<IUser>> list);

        /// <summary>
        /// 移除指定角色中的指定用户`
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="users">用户</param>
        void RemoveUserToRole(IRole role, params IUser[] users);

        /// <summary>
        /// 移除指定角色中的指定用户
        /// </summary>
        /// <param name="list">用户角色匹配列表，Key为角色，Value为用户</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        void RemoveUsersToRoles(IDictionary<IRole, IEnumerable<IUser>> list);

        /// <summary>
        /// 在数据源中为添加一个新角色
        /// </summary>
        /// <param name="roleCode">要创建的角色的代码</param>
        void CreateRole(IRole role);

        /// <summary>
        /// 从数据源中移除角色
        /// </summary>
        /// <param name="roleCode">要删除的角色的代码</param>
        /// <param name="throwOnPopulatedRole">如果为 true，则在 roleCode 具有一个或多个成员时引发异常，并且不删除 roleCode</param>
        /// <returns>如果成功删除角色，则为 true；否则为 false</returns>
        bool DeleteRole(string roleCode, bool throwOnPopulatedRole);

        /// <summary>
        /// 获取所有角色的列表
        /// </summary>
        /// <returns>角色列表</returns>
        IEnumerable<IRole> AllRoles { get; }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleCode">角色的代码</param>
        /// <returns>角色</returns>
        IRole GetRole(string roleCode);

        /// <summary>
        /// 获取指定用户所属于的角色的列表
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <returns>角色列表</returns>
        IEnumerable<IRole> GetRolesForUser(string logOnName);

        /// <summary>
        /// 获取属于指定角色的用户的列表
        /// </summary>
        /// <param name="roleCode">角色名称</param>
        /// <returns>用户列表</returns>
        IEnumerable<IUser> GetUsersInRole(string roleCode);

        /// <summary>
        /// 指示指定用户是否属于指定角色
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <param name="roleCode">角色代码</param>
        /// <returns>如果指定用户属于指定角色，则为 true；否则为 false</returns>
        bool IsUserInRole(string logOnName, string roleCode);

        /// <summary>
        /// 指示指定角色名是否已存在于角色数据源中
        /// </summary>
        /// <param name="roleCode">角色代码</param>
        /// <returns>如果角色名已存在于数据源中，则为 true；否则为 false</returns>
        bool RoleExists(string roleCode);

        /// <summary>
        /// 应用程序的名称
        /// </summary>
        string ApplicationName { get; }
    }
}