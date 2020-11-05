using YJC.Toolkit.IM;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp
{
    public abstract class BaseXmlWeCorpPlatformSettingCreator :
        BaseXmlAppPlatformSettingCreator<WeCorpSettings>, IWeCorpPlatformSettingCreator
    {
        protected BaseXmlWeCorpPlatformSettingCreator(string fileName)
            : base(fileName)
        {
        }

        #region IWeCorpPlatformSettingCreator 成员

        public abstract WeCorpSettings CreateWithCorpId(string corpId);

        #endregion IWeCorpPlatformSettingCreator 成员

        protected override string GetAppName(string tenantId, object customData)
        {
            string appName = customData.ConvertToString();
            TkDebug.AssertNotNullOrEmpty(appName, "API调用错误，应该传入AppName", this);

            return appName;
        }
    }
}