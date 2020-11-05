using YJC.Toolkit.Sys;

namespace YJC.Toolkit.DingTalk
{
    public class DingTalkAppConfig : IRegName
    {
        protected DingTalkAppConfig()
        {
        }

        public DingTalkAppConfig(string shortName, int agentId, string appName,
            string appKey, string appSecret)
        {
            TkDebug.AssertArgumentNullOrEmpty(shortName, "shortName", null);
            TkDebug.AssertArgumentNullOrEmpty(appName, "appName", null);
            TkDebug.AssertArgumentNullOrEmpty(appKey, "appKey", null);
            TkDebug.AssertArgumentNullOrEmpty(appSecret, "appSecret", null);

            ShortName = shortName;
            AgentId = agentId;
            AppName = appName;
            AppKey = appKey;
            AppSecret = appSecret;
        }

        #region IRegName 成员

        public string RegName
        {
            get
            {
                return ShortName;
            }
        }

        #endregion IRegName 成员

        [SimpleAttribute]
        public string ShortName { get; private set; }

        [SimpleAttribute]
        public int AgentId { get; private set; }

        [SimpleAttribute]
        public string AppName { get; private set; }

        [SimpleAttribute]
        public string AppKey { get; private set; }

        [SimpleAttribute]
        public string AppSecret { get; private set; }
    }
}