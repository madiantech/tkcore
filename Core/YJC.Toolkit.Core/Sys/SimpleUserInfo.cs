using System.Collections;
using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public class SimpleUserInfo : IUserInfo
    {
        private const int PAGE_SIZE = 15;

        public SimpleUserInfo()
        {
            IsLogOn = false;
            PageSize = PAGE_SIZE;
        }

        public SimpleUserInfo(string userName, string logOnName, object userId, object mainOrgId)
        {
            UserName = userName;
            LogOnName = logOnName;
            UserId = userId;
            MainOrgId = mainOrgId;
            IsLogOn = true;
            PageSize = PAGE_SIZE;
        }

        public SimpleUserInfo(string userName, string logOnName, object userId,
            object mainOrgId, IEnumerable<string> roleIds)
            : this(userName, logOnName, userId, mainOrgId)
        {
            if (roleIds != null)
            {
                InternalRoleIds = new List<string>();
                InternalRoleIds.AddRange(roleIds);
            }
        }

        #region IUserInfo 成员

        [SimpleAttribute]
        public string UserName { get; protected set; }

        [SimpleAttribute]
        public string LogOnName { get; protected set; }

        //public string Encoding { get; private set; }

        [SimpleAttribute(ObjectType = typeof(string))]
        public object UserId { get; protected set; }

        [SimpleAttribute(ObjectType = typeof(string))]
        public object MainOrgId { get; protected set; }

        public IEnumerable OtherOrgs
        {
            get
            {
                return null;
            }
        }

        public IEnumerable RoleIds
        {
            get
            {
                return InternalRoleIds;
            }
        }

        [SimpleAttribute]
        public int PageSize { get; set; }

        public bool IsLogOn { get; protected set; }

        public object Data1 { get; set; }

        public object Data2 { get; set; }

        #endregion IUserInfo 成员

        [SimpleElement(IsMultiple = true, LocalName = "RoleId")]
        private List<string> InternalRoleIds { get; set; }
    }
}