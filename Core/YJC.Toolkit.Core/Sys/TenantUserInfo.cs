using System.Collections.Generic;

namespace YJC.Toolkit.Sys
{
    public class TenantUserInfo : SimpleUserInfo, ISupportTenant
    {
        public TenantUserInfo()
        {
        }

        public TenantUserInfo(string userName, string logOnName, object userId, object mainOrgId, object tenantId)
            : base(userName, logOnName, userId, mainOrgId)
        {
            TenantId = tenantId;
        }

        public TenantUserInfo(string userName, string logOnName, object userId, object mainOrgId,
            IEnumerable<string> roleIds, object tenantId)
            : base(userName, logOnName, userId, mainOrgId, roleIds)
        {
            TenantId = tenantId;
        }

        public TenantUserInfo(IUserInfo user, object tenantId)
            : this(user.UserName, user.LogOnName, user.UserId, user.MainOrgId, user.RoleIds)
        {
            TenantId = tenantId;
        }

        [SimpleAttribute(ObjectType = typeof(string))]
        public object TenantId { get; protected set; }
    }
}