using YJC.Toolkit.IM;

namespace YJC.Toolkit.WeCorp
{
    public interface IWeCorpSettingCreator
    {
        WeCorpSettings Create(string tenantId);

        AccessToken ReadToken(string tenantId);

        void SaveToken(string tenantId, AccessToken token);
    }
}