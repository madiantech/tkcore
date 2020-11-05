using System;
using System.Collections.Generic;
using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public interface IUser : IEntity
    {
        /// <summary>
        /// 更新成员资格数据存储区中成员资格用户的密码。
        /// </summary>
        /// <param name="oldPassword">成员资格用户的当前密码</param>
        /// <param name="newPassword">成员资格用户的新密码</param>
        /// <returns>如果更新成功，则为 true；否则为 false</returns>
        bool ChangePassword(string oldPassword, string newPassword);

        /// <summary>
        /// 更新成员资格数据存储区中成员资格用户的密码提示问题和密码提示问题答案。
        /// </summary>
        /// <param name="password">成员资格用户的当前密码</param>
        /// <param name="newPasswordQuestion">成员资格用户的新密码提示问题的值</param>
        /// <param name="newPasswordAnswer">成员资格用户的新密码提示问题答案的值</param>
        /// <returns>如果更新成功，则为 true；否则为 false</returns>
        bool ChangePasswordQuestionAndAnswer(string password, string newPasswordQuestion, string newPasswordAnswer);

        /// <summary>
        /// 从成员资格数据存储区获取成员资格用户的密码。
        /// </summary>
        /// <returns>成员资格用户的密码</returns>
        string GetPassword();

        /// <summary>
        /// 从成员资格数据存储区获取成员资格用户的密码，需要回答密码提示问题。
        /// </summary>
        /// <param name="passwordAnswer">成员资格用户的密码提示问题答案</param>
        /// <returns>成员资格用户的密码</returns>
        string GetPassword(string passwordAnswer);

        /// <summary>
        /// 将用户密码重置为一个自动生成的新密码。
        /// </summary>
        /// <returns>成员资格用户的新密码</returns>
        string ResetPassword();

        /// <summary>
        /// 将用户密码重置为一个自动生成的新密码，需要回答密码提示问题。
        /// </summary>
        /// <param name="passwordAnswer">成员资格用户的密码提示问题答案</param>
        /// <returns>成员资格用户的新密码</returns>
        string ResetPassword(string passwordAnswer);

        /// <summary>
        /// 清除用户的锁定状态以便可以验证成员资格用户。
        /// </summary>
        /// <returns>如果成功取消成员资格用户的锁定，则为 true；否则为 false</returns>
        bool UnlockUser();

        /// <summary>
        /// 获取或设置成员资格用户的登录名。
        /// </summary>
        string LogOnName { get; set; }

        /// <summary>
        /// 获取将用户添加到成员资格数据存储区的日期和时间。
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// 工号
        /// </summary>
        string Number { get; set; }

        /// <summary>
        /// 获取或设置成员资格用户的电子邮件地址。
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否可以对成员资格用户进行身份验证。
        /// </summary>
        bool IsApproved { get; set; }

        /// <summary>
        /// 获取一个值，该值指示成员资格用户是否因被锁定而无法进行验证。
        /// </summary>
        bool IsLockedOut { get; }

        /// <summary>
        /// 获取一个值，表示用户当前是否联机。
        /// </summary>
        bool IsOnline { get; }

        /// <summary>
        /// 获取或设置成员资格用户上次进行身份验证或访问应用程序的日期和时间。
        /// </summary>
        DateTime LastActivityDate { get; set; }

        /// <summary>
        /// 获取最近一次锁定成员资格用户的日期和时间。
        /// </summary>
        DateTime LastLockoutDate { get; }

        /// <summary>
        /// 获取或设置用户上次进行身份验证的日期和时间。
        /// </summary>
        DateTime LastLogOnDate { get; set; }

        /// <summary>
        /// 获取上次更新成员资格用户的密码的日期和时间。
        /// </summary>
        DateTime LastPasswordChangedDate { get; }

        /// <summary>
        /// 获取成员资格用户的密码提示问题。
        /// </summary>
        string PasswordQuestion { get; }

        string MainOrgId { get; }

        string ManagerId { get; }

        string GetImId(string type);

        IUser Manager { get; }

        IOrganization MainOrg { get; }

        IEnumerable<IRole> Roles { get; }

        IEnumerable<IOrganization> GetAllOrgs(bool includeMain);
    }
}