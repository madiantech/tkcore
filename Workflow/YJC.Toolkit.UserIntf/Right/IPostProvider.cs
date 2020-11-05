using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IPostProvider : ITree
    {
        /// <summary>
        /// 获取岗位
        /// </summary>
        /// <param name="postCode">岗位代码</param>
        /// <returns>岗位</returns>
        IPost GetPost(string postCode);

        /// <summary>
        /// 获取父岗位
        /// </summary>
        /// <param name="postCode">岗位代码</param>
        /// <returns>父岗位</returns>
        IPost GetParentPost(string postCode);

        IPost GetGrandParentPost(string postCode);

        /// <summary>
        /// 获取指定用户所属于的岗位的列表
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <returns>岗位列表</returns>
        IEnumerable<IPost> GetPostsForUser(string logOnName);

        /// <summary>
        /// 获取指定用户所属于的主岗位
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <returns>岗位</returns>
        IPost GetMainPostsForUser(string logOnName);

        /// <summary>
        /// 获取属于指定岗位的用户的列表
        /// </summary>
        /// <param name="postCode">岗位代码</param>
        /// <returns>用户列表</returns>
        IEnumerable<IUser> GetUsersInPost(string postCode);

        /// <summary>
        /// 获取属于指定主岗位的用户
        /// </summary>
        /// <param name="postCode">岗位代码</param>
        /// <returns>用户</returns>
        IUser GetUserInMainPost(string postCode);

        /// <summary>
        /// 获取属于指定岗位和指定角色的用户的列表
        /// </summary>
        /// <param name="postCode">岗位代码</param>
        /// <param name="roleCode">角色代码</param>
        /// <returns>用户列表</returns>
        IEnumerable<IUser> FindPostUsersByRole(string postCode, string roleCode);

        IEnumerable<IUser> FindPostUsersByOrg(string postCode, string orgCode);

        /// <summary>
        /// 获取属于指定主岗位和指定角色的用户
        /// </summary>
        /// <param name="postCode">岗位代码</param>
        /// <param name="roleCode">角色代码</param>
        /// <returns>用户</returns>
        IUser FindUserInMainPost(string postCode, string roleCode);

        /// <summary>
        /// 指示指定用户是否属于指定岗位
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <param name="postCode">岗位代码</param>
        /// <returns>如果指定用户属于指定岗位，则为 true；否则为 false</returns>
        bool IsUserInPost(string logOnName, string postCode);

        /// <summary>
        /// 获取指定机构所属于的岗位的列表
        /// </summary>
        /// <param name="orgCode">机构代码</param>
        /// <returns>岗位列表</returns>
        IEnumerable<IPost> GetPostsForOrganization(string orgCode);

        /// <summary>
        /// 获取指定岗位的所属机构
        /// </summary>
        /// <param name="postCode">岗位代码</param>
        /// <returns>所属机构</returns>
        IOrganization GetOrganizationOfPost(string postCode);

        /// <summary>
        /// 应用程序的名称
        /// </summary>
        string ApplicationName { get; }
    }
}