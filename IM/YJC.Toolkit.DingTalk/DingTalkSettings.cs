using YJC.Toolkit.Collections;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk
{
    public class DingTalkSettings
    {
        private readonly RegNameList<DingTalkAppConfig> fApps;

        protected DingTalkSettings()
        {
            fApps = new RegNameList<DingTalkAppConfig>();
        }

        public DingTalkSettings(DingTalkAppConfig app)
            : this()
        {
            TkDebug.AssertArgumentNull(app, "app", null);

            DefaultAppName = app.AppName;
            fApps.Add(app);
        }

        public DingTalkSettings(string defaultAppName, params DingTalkAppConfig[] apps)
            : this()
        {
            TkDebug.AssertArgumentNullOrEmpty(defaultAppName, "defaultAppName", null);
            TkDebug.AssertEnumerableArgumentNull(apps, "apps", null);

            DefaultAppName = defaultAppName;
            bool isValidName = false;
            foreach (var item in apps)
            {
                fApps.Add(item);
                if (item.ShortName == defaultAppName)
                    isValidName = true;
            }
            TkDebug.Assert(isValidName, string.Format(ObjectUtil.SysCulture,
                "定义的{0}缺省AppName，并不存在，请确认", defaultAppName), null);
        }

        public DingTalkAppConfig this[string shortName]
        {
            get
            {
                TkDebug.AssertArgumentNullOrEmpty(shortName, "shortName", this);
                TkDebug.AssertArgument(fApps.ConstainsKey(shortName), "shortName",
                    string.Format(ObjectUtil.SysCulture, "当前配置中没有短名称为{0}的钉钉App配置，请确认", shortName), this);

                return fApps[shortName];
            }
        }

        public void Add(DingTalkAppConfig app)
        {
            TkDebug.AssertArgumentNull(app, "app", this);

            fApps.Add(app);
        }

        public string DefaultAppName { get; private set; }

        //public DingTalkSettings(string corpId, string corpSecret, string ssoSecret)
        //{
        //    TkDebug.AssertArgumentNullOrEmpty(corpId, "corpId", null);
        //    TkDebug.AssertArgumentNullOrEmpty(corpSecret, "corpSecret", null);
        //    TkDebug.AssertArgumentNullOrEmpty(ssoSecret, "ssoSecret", null);

        //    CorpId = corpId;
        //    CorpSecret = corpSecret;
        //    SsoSecret = ssoSecret;
        //}

        //public string CorpId { get; protected set; }

        //public string CorpSecret { get; protected set; }

        //public string SsoSecret { get; protected set; }

        //public string Token { get; protected set; }

        //public string AesKey { get; protected set; }

        //public void SetCallbackParam(string token, string aesKey)
        //{
        //    TkDebug.AssertArgumentNullOrEmpty(token, "token", null);
        //    TkDebug.AssertArgumentNullOrEmpty(aesKey, "aesKey", null);

        //    Token = token;
        //    AesKey = aesKey;
        //}
    }
}