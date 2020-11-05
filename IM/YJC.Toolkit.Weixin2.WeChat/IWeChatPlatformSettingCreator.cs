using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeChat
{
    public interface IWeChatPlatformSettingCreator : IPlatformSettingCreator<WeChatSettings>
    {
        WeChatSettings CreateWithOpenId(string openId);
    }
}