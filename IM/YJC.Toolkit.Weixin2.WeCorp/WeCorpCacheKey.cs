using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp
{
    public sealed class WeCorpCacheKey
    {
        private WeCorpCacheKey()
        {
        }

        public WeCorpCacheKey(string tenantId, string appName)
        {
            TenantId = tenantId;
            AppName = appName;
        }

        [SimpleAttribute]
        public string TenantId { get; private set; }

        [SimpleAttribute]
        public string AppName { get; private set; }

        public string ToJson()
        {
            return this.WriteJson();
        }

        public static WeCorpCacheKey ReadFromString(string json)
        {
            TkDebug.AssertArgumentNullOrEmpty(json, "json", null);

            WeCorpCacheKey key = new WeCorpCacheKey();
            key.ReadJson(json);
            return key;
        }
    }
}