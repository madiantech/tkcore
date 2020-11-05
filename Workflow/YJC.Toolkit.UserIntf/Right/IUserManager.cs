namespace YJC.Toolkit.Right
{
    public interface IUserManager
    {
        bool SupportPost { get; }

        bool SupportDepartment { get; }

        bool SupportPasswdQuestion { get; }

        IUserProvider UserProvider { get; }

        IRoleProvider RoleProvider { get; }

        IOrganizationProvider OrgProvider { get; }

        IPostProvider PostProvider { get; }

        /// <summary>
        /// 应用程序的名称
        /// </summary>
        string ApplicationName { get; }
    }
}