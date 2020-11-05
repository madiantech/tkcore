using System.Runtime.CompilerServices;
using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeChat
{
    internal class WeChatPlatformConfiguration : PlatformConfiguration<WeChatSettings>
    {
        private IWeChatPlatformSettingCreator fCreator;

        public WeChatSettings CreateWithOpenId(string openId)
        {
            return fCreator.CreateWithOpenId(openId);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetWeChatCreator(IWeChatPlatformSettingCreator creator)
        {
            fCreator = creator;
            SetCreator(creator);
        }
    }
}