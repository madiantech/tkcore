using System;
using System.Data;
using YJC.Toolkit.Sys;
using System.Collections.Generic;

namespace YJC.Toolkit.Right
{
    internal class User : IUser
    {
        private readonly UserProvider fProvider;
        private readonly int fUserActive;

        internal User(DataRow row, UserProvider provider)
        {
            Id = row["Id"].ToString();
            Name = row["Name"].ToString();
            LogOnName = row["LoginName"].ToString();
            LastLogOnDate = row["LoginDate"].Value<DateTime>();
            Phone = row["Phone"].ToString();
            Email = row["Email"].ToString();
            Number = row["WorkNo"].ToString();
            CreationDate = row["CreateDate"].Value<DateTime>();
            LastPasswordChangedDate = row["PasswdChangeDate"].Value<DateTime>();
            fUserActive = row["Active"].Value<int>();
            LastLockoutDate = row["UnlockDate"].Value<DateTime>();
            MainOrgId = row["OrgId"].ToString();

            fProvider = provider;
            //是否启用
            IsApproved = fUserActive == 1;
            PasswordQuestion = string.Empty;
        }

        #region IUser 成员

        public string ApplicationName
        {
            get
            {
                return string.Empty;
            }
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return fProvider.ChangePassword(LogOnName, oldPassword, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return fProvider.ChangePasswordQuestionAndAnswer(LogOnName, password, newPasswordQuestion, newPasswordAnswer);
        }

        [SimpleAttribute]
        public DateTime CreationDate { get; private set; }

        [SimpleAttribute]
        public string Email { get; set; }

        public string GetPassword(string passwordAnswer)
        {
            return fProvider.GetPassword(LogOnName, passwordAnswer);
        }

        public string GetPassword()
        {
            return GetPassword(string.Empty);
        }

        [SimpleAttribute]
        public bool IsApproved { get; set; }

        public bool IsLockedOut
        {
            get
            {
                return fUserActive != 1;
            }
        }

        public bool IsOnline
        {
            get
            {
                return true;
            }
        }

        [SimpleAttribute]
        public DateTime LastActivityDate { get; set; }

        [SimpleAttribute]
        public DateTime LastLockoutDate { get; private set; }

        [SimpleAttribute]
        public DateTime LastLogOnDate { get; set; }

        [SimpleAttribute]
        public DateTime LastPasswordChangedDate { get; private set; }

        [SimpleAttribute]
        public string LogOnName { get; set; }

        [SimpleAttribute]
        public string Number { get; set; }

        [SimpleAttribute]
        public string PasswordQuestion { get; private set; }

        [SimpleAttribute]
        public string Phone { get; set; }

        [SimpleAttribute]
        public string MainOrgId { get; private set; }

        public string ManagerId
        {
            get
            {
                return null;
            }
        }

        public string GetImId(string type)
        {
            return string.Empty;
        }

        public IUser Manager
        {
            get
            {
                return null;
            }
        }

        public IOrganization MainOrg
        {
            get
            {
                if (string.IsNullOrEmpty(MainOrgId))
                    return null;
                else
                    return fProvider.Manager.OrgProvider.GetMainOrganizationsForUser(LogOnName);
            }
        }

        public IEnumerable<IRole> Roles
        {
            get
            {
                return fProvider.Manager.RoleProvider.GetRolesForUser(LogOnName);
            }
        }

        public IEnumerable<IOrganization> GetAllOrgs(bool includeMain)
        {
            return EnumUtil.Convert(MainOrg);
        }

        public string ResetPassword(string passwordAnswer)
        {
            return ResetPassword();
        }

        public string ResetPassword()
        {
            return fProvider.ResetPassword(LogOnName, string.Empty);
        }

        public bool UnlockUser()
        {
            return fProvider.UnlockUser(LogOnName);
        }

        #endregion IUser 成员

        #region IEntity 成员

        [SimpleAttribute]
        public string Id { get; private set; }

        [SimpleAttribute]
        public string Name { get; private set; }

        #endregion IEntity 成员
    }
}