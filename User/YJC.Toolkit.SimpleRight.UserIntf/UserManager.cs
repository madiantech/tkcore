using YJC.Toolkit.Data;

namespace YJC.Toolkit.Right
{
    public class UserManager : IUserManager
    {
        private readonly DbContextConfig fDbConfig;
        private IOrganizationProvider fOrgProvider;
        private IRoleProvider fRoleProvider;
        private IUserProvider fUserProvider;

        public UserManager(DbContextConfig dbConfig)
        {
            fDbConfig = dbConfig;
        }

        #region IUserManager 成员

        public string ApplicationName
        {
            get
            {
                return string.Empty;
            }
        }

        public IOrganizationProvider OrgProvider
        {
            get
            {
                if (fOrgProvider == null)
                {
                    fOrgProvider = new OrganizationProvider(this, fDbConfig);
                }
                return fOrgProvider;
            }
        }

        public IPostProvider PostProvider
        {
            get
            {
                return null;
            }
        }

        public IRoleProvider RoleProvider
        {
            get
            {
                if (fRoleProvider == null)
                {
                    fRoleProvider = new RoleProvider(this, fDbConfig);
                }
                return fRoleProvider;
            }
        }

        public bool SupportDepartment
        {
            get
            {
                return false;
            }
        }

        public bool SupportPasswdQuestion
        {
            get
            {
                return false;
            }
        }

        public bool SupportPost
        {
            get
            {
                return false;
            }
        }

        public IUserProvider UserProvider
        {
            get
            {
                if (fUserProvider == null)
                {
                    fUserProvider = new UserProvider(this, fDbConfig);
                }
                return fUserProvider;
            }
        }

        #endregion IUserManager 成员
    }
}