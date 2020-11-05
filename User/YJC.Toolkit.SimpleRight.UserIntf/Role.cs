using System.Data;
using System.Collections.Generic;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Right
{
    internal class Role : IRole
    {
        private readonly IRoleProvider fRoleProvider;
        private IEnumerable<IUser> fUsers;
        private bool fIsGetUsers;

        public Role(DataRow row, IRoleProvider roleProvider)
        {
            fRoleProvider = roleProvider;
            Id = row["Id"].ToString();
            Name = row["Name"].ToString();
            RoleCode = Id;
        }

        #region IRole 成员

        [SimpleAttribute]
        public string RoleCode { get; private set; }

        public IEnumerable<IUser> Users
        {
            get
            {
                if (!fIsGetUsers)
                {
                    fIsGetUsers = true;
                    fUsers = fRoleProvider.GetUsersInRole(Id);
                }
                return fUsers;
            }
        }

        #endregion IRole 成员

        #region IEntity 成员

        [SimpleAttribute]
        public string Id { get; private set; }

        [SimpleAttribute]
        public string Name { get; private set; }

        #endregion IEntity 成员
    }
}