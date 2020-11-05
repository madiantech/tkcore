using System.Runtime.CompilerServices;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeCorp
{
    public class WeCorpConfiguration
    {
        private class InternalConfiguration : PlatformConfiguration<WeCorpSettings>
        {
            private IWeCorpPlatformSettingCreator fCreator;

            public WeCorpSettings CreateWithCorpId(string corpId)
            {
                return fCreator.CreateWithCorpId(corpId);
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public void SetWeChatCreator(IWeCorpPlatformSettingCreator creator)
            {
                fCreator = creator;
                SetCreator(creator);
            }
        }

        private static readonly InternalConfiguration fConfig = new InternalConfiguration();

        public static WeCorpSettings Create(string tenantId)
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
        public static void SetCreator(IPlatformSettingCreator<WeCorpSettings> creator)//(IWeCorpPlatformSettingCreator creator)//IPlatformSettingCreator<WeCorpSettings> creator
        {
            fConfig.SetCreator(creator);
        }
    }
}