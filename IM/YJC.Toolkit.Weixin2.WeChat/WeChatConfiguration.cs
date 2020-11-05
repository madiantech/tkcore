using System.Runtime.CompilerServices;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeChat
{
    public class WeChatConfiguration
    {
        private readonly static WeChatPlatformConfiguration fConfig = new WeChatPlatformConfiguration();

        public static WeChatSettings Create(string tenantId)
        {
            return fConfig.Create(tenantId);
        }

        public static WeChatSettings Create()
        {
            return fConfig.Create(null);
        }

        public static WeChatSettings CreateWithOpenId(string openId)
        {
            return fConfig.CreateWithOpenId(openId);
        }

        public static AccessToken ReadToken(string tenantId)
        {
            return fConfig.ReadToken(tenantId, null);
        }

        public static void SaveToken(string tenantId, AccessToken token)
        {
            fConfig.SaveToken(tenantId, null, token);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SetCreator(IWeChatPlatformSettingCreator creator)
        {
            fConfig.SetCreator(creator);
        }
    }
}