using YJC.Toolkit.Data;

namespace YJC.Toolkit.Sys
{
    public static class UserInfoExtension
    {
        public static bool IsSupportTenant(this IUserInfo info)
        {
            return info is ISupportTenant;
        }

        public static object GetTenantValue(this IUserInfo info, bool throwIfNotSupport = true)
        {
            TkDebug.AssertArgumentNull(info, nameof(info), null);

            if (info is ISupportTenant support)
                return support.TenantId;

            if (throwIfNotSupport)
                throw new ToolkitException($"当前的UserInfo类型是{info.GetType()}，不支持接口ISupportTenant", info);
            return null;
        }
    }
}