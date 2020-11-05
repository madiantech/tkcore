using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeCorp
{
    public interface IWeCorpPlatformSettingCreator : IPlatformSettingCreator<WeCorpSettings>
    {
        WeCorpSettings CreateWithCorpId(string corpId);
    }
}