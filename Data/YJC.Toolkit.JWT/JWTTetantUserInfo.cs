using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public class JWTTetantUserInfo : JWTUserInfo, ISupportTenant
    {
        public JWTTetantUserInfo()
        {
        }

        public JWTTetantUserInfo(IUserInfo info) : base(info)
        {
            TenantId = info.GetTenantValue();
        }

        [SimpleAttribute(ObjectType = typeof(string))]
        public object TenantId { get; private set; }
    }
}