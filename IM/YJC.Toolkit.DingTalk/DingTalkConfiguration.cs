using System.Runtime.CompilerServices;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.DingTalk
{
    public class DingTalkConfiguration
    {
        private class InternalConfiguration : PlatformConfiguration<DingTalkSettings>
        {
        }

        private readonly static InternalConfiguration fConfig = new InternalConfiguration();

        public static DingTalkSettings Create(string tenantId)
        {
            return fConfig.Create(tenantId);
        }

        public static AccessToken ReadToken(string tenantId, string appName)
        {
            return fConfig.ReadToken(tenantId, appName);
        }

        public static void SaveToken(string tenantId, string appName, AccessToken token)
        {
            fConfig.SaveToken(tenantId, appName, token);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void SetCreator(IPlatformSettingCreator<DingTalkSettings> creator)
        {
            fConfig.SetCreator(creator);
        }
    }
}