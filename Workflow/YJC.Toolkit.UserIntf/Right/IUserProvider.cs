using System.Collections.Generic;

namespace YJC.Toolkit.Right
{
    public interface IUserProvider
    {
        /// <summary>
        /// 处理更新成员资格用户密码的请求。
        /// </summary>
        /// <param name="logOnName">为其更新密码的用户登录名</param>
        /// <param name="oldPassword">指定的用户的当前密码</param>
        /// <param name="newPassword">指定的用户的新密码</param>
        /// <returns>如果密码更新成功，则为 true；否则为 false</returns>
        bool ChangePassword(string logOnName, string oldPassword, string newPassword);

        /// <summary>
        /// 处理更新成员资格用户的密码提示问题和答案的请求。
        /// </summary>
        /// <param name="logOnName">要为其更改密码提示问题和答案的用户登录名</param>
        /// <param name="password">指定的用户的密码</param>
        /// <param name="newPasswordQuestion">指定的用户的新密码提示问题</param>
        /// <param name="newPasswordAnswer">指定的用户的新密码提示问题答案</param>
        /// <returns>如果成功更新密码提示问题和答案，则为 true；否则，为 false</returns>
        bool ChangePasswordQuestionAndAnswer(string logOnName, string password, string newPasswordQuestion, string newPasswordAnswer);

        /// <summary>
        /// 将新的成员资格用户添加到数据源。
        /// </summary>
        /// <param name="user">新用户</param>
        void CreateUser(IUser user);

        /// <summary>
        /// 从成员资格数据源删除一个用户。
        /// </summary>
        /// <param name="logOnName">要删除的用户登录名</param>
        /// <param name="deleteAllRelatedData">如果为 true，则从数据库中删除与该用户相关的数据；如果为 false，则将与该用户相关的数据保留在数据库</param>
        /// <returns>如果用户被成功删除，则为 true；否则为 false</returns>
        bool DeleteUser(string logOnName, bool deleteAllRelatedData);

        /// <summary>
        /// 获取一个成员资格用户的集合，其中的电子邮件地址包含要匹配的指定电子邮件地址。
        /// </summary>
        /// <param name="emailToMatch">要搜索的电子邮件地址</param>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 是从零开始的</param>
        /// <param name="pageSize">要返回的结果页的大小</param>
        /// <param name="totalRecords">匹配用户的总数</param>
        /// <returns>用户的集合</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        IEnumerable<IUser> FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取一个成员资格用户的集合，其中的电话号码包含要匹配的指定电话号码。
        /// </summary>
        /// <param name="phoneToMatch">要搜索的电话号码</param>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 是从零开始的</param>
        /// <param name="pageSize">要返回的结果页的大小</param>
        /// <param name="totalRecords">匹配用户的总数</param>
        /// <returns>用户的集合</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        IEnumerable<IUser> FindUsersByPhone(string phoneToMatch, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取一个成员资格用户的集合，其中的工号包含要匹配的指定工号。
        /// </summary>
        /// <param name="numberToMatch">要搜索的工号</param>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 是从零开始的</param>
        /// <param name="pageSize">要返回的结果页的大小</param>
        /// <param name="totalRecords">匹配用户的总数</param>
        /// <returns>用户的集合</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        IEnumerable<IUser> FindUsersByNumber(string numberToMatch, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取一个成员资格用户的集合，其中的用户名包含要匹配的指定用户姓名
        /// </summary>
        /// <param name="fullnameToMatch">用户姓名</param>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 是从零开始的</param>
        /// <param name="pageSize">要返回的结果页的大小</param>
        /// <param name="totalRecords">匹配用户的总数</param>
        /// <returns>用户的集合</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        IEnumerable<IUser> FindUsersByName(string fullnameToMatch, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取数据源中的所有用户的集合，并显示在数据页中。
        /// </summary>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 是从零开始的</param>
        /// <param name="pageSize">要返回的结果页的大小</param>
        /// <param name="totalRecords">匹配用户的总数</param>
        /// <returns>用户的集合</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        IEnumerable<IUser> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取指定用户所在组织的所有子组织的用户集合（不包括用户所在组织的用户，但是包含用户自身）
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <returns>用户的集合</returns>
        IEnumerable<IUser> GetAllUsersInDescendantOrganization(string logOnName);

        /// <summary>
        /// 获取当前访问该应用程序的用户数。
        /// </summary>
        /// <returns>当前访问该应用程序的用户数</returns>
        int NumberOfUsersOnline { get; }

        /// <summary>
        /// 从数据源获取指定用户名所对应的密码。
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <param name="answer">用户的密码提示问题答案</param>
        /// <returns>密码</returns>
        string GetPassword(string logOnName, string answer);

        /// <summary>
        /// 从数据源获取用户的信息。提供一个更新用户最近一次活动的日期/时间戳的选项。
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <param name="userIsOnline">如果为 true，则更新用户最近一次活动的日期/时间戳；如果为 false，则返回用户信息，但不更新用户最近一次活动的日期/时间戳。</param>
        /// <returns>用户</returns>
        IUser GetUser(string logOnName, bool userIsOnline);

        IUser GetUser(string userId);

        /// <summary>
        /// 获取与指定的电子邮件地址关联的用户名。
        /// </summary>
        /// <param name="email">要搜索的电子邮件地址</param>
        /// <returns>用户名。如果未找到匹配项，则返回 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing）</returns>
        string GetUserNameByEmail(string email);

        /// <summary>
        /// 将用户密码重置为一个自动生成的新密码。
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <param name="answer">密码提示问题答案</param>
        /// <returns>新密码</returns>
        string ResetPassword(string logOnName, string answer);

        /// <summary>
        /// 清除锁定，以便可以验证该成员资格用户。
        /// </summary>
        /// <param name="logOnName">要清除其锁定状态的成员资格用户登录名</param>
        /// <returns>如果成功取消成员资格用户的锁定，则为 true；否则为 false</returns>
        bool UnlockUser(string logOnName);

        /// <summary>
        /// 更新数据源中有关用户的信息。
        /// </summary>
        /// <param name="user">用户</param>
        void UpdateUser(IUser user);

        /// <summary>
        /// 验证数据源中是否存在指定的用户名和密码。
        /// </summary>
        /// <param name="logOnName">用户登录名</param>
        /// <param name="password">密码</param>
        /// <returns>如果指定的用户名和密码有效，则为 true；否则为 false</returns>
        bool ValidateUser(string logOnName, string password);

        /// <summary>
        /// 应用程序的名称
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// 指示成员资格提供程序是否配置为允许用户重置其密码。
        /// </summary>
        bool EnablePasswordReset { get; }

        /// <summary>
        /// 指示成员资格提供程序是否配置为允许用户检索其密码。
        /// </summary>
        bool EnablePasswordRetrieval { get; }

        /// <summary>
        /// 获取锁定成员资格用户前允许的无效密码或无效密码提示问题答案尝试次数。
        /// </summary>
        int MaxInvalidPasswordAttempts { get; }

        /// <summary>
        /// 获取在锁定成员资格用户之前允许的最大无效密码或无效密码提示问题答案尝试次数的分钟数。
        /// </summary>
        int PasswordAttemptWindow { get; }

        /// <summary>
        /// 获取一个值，该值指示成员资格提供程序是否配置为要求用户在进行密码重置和检索时回答密码提示问题。
        /// </summary>
        bool RequiresQuestionAndAnswer { get; }

        /// <summary>
        /// 获取一个值，指示成员资格提供程序是否配置为要求每个用户名具有唯一的电子邮件地址。
        /// </summary>
        bool RequiresUniqueEmail { get; }

        IPasswordProvider PasswordProvider { get; }
    }
}