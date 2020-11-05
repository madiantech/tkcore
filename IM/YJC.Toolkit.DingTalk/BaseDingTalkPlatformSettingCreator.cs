using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk
{
    public abstract class BaseDingTalkPlatformSettingCreator
        : BaseXmlAppPlatformSettingCreator<DingTalkSettings>
    {
        protected BaseDingTalkPlatformSettingCreator(string fileName)
            : base(fileName)
        {
        }

        protected override string GetAppName(string tenantId, object customData)
        {
            string appName = customData.ConvertToString();
            return appName ?? IMUtil.NULL_KEY;
        }
    }
}