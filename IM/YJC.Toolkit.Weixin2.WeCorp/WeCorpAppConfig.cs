using YJC.Toolkit.Sys;

namespace YJC.Toolkit.WeCorp
{
    public class WeCorpAppConfig : IRegName
    {
        internal WeCorpAppConfig()
        {
        }

        public WeCorpAppConfig(string shortName, int appId, string appName, string secret,
            string token, string aesKey)
        {
            TkDebug.AssertArgumentNullOrEmpty(shortName, "shortName", null);
            TkDebug.AssertArgumentNullOrEmpty(appName, "appName", null);
            TkDebug.AssertArgumentNullOrEmpty(secret, "secret", null);
            TkDebug.AssertArgumentNullOrEmpty(token, "token", null);
            TkDebug.AssertArgumentNullOrEmpty(aesKey, "aesKey", null);

            ShortName = shortName;
            AppId = appId;
            AppName = appName;
            Secret = secret;
            Token = token;
            AesKey = aesKey;
        }

        #region IRegName 成员

        public string RegName => ShortName;

        #endregion IRegName 成员

        [SimpleAttribute]
        public string ShortName { get; private set; }

        [SimpleAttribute]
        public int AppId { get; private set; }

        [SimpleAttribute]
        public string AppName { get; private set; }

        [SimpleAttribute]
        public string Secret { get; private set; }

        [SimpleAttribute]
        public string Token { get; private set; }

        [SimpleAttribute]
        public string AesKey { get; private set; }
    }
}