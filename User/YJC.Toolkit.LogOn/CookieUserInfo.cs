using YJC.Toolkit.Sys;

namespace YJC.Toolkit.LogOn
{
    public sealed class CookieUserInfo
    {
        private CookieUserInfo()
        {
        }

        public CookieUserInfo(LogOnData logOnData, IUserInfo userInfo)
        {
            TkDebug.AssertArgumentNull(logOnData, "logOnData", null);
            TkDebug.AssertArgumentNull(userInfo, "userInfo", null);

            UserId = ObjectUtil.ToString(userInfo.UserId, BaseAppSetting.Current.WriteSettings);
            Password = logOnData.Password;
        }

        [SimpleAttribute]
        public string UserId { get; private set; }

        [SimpleAttribute]
        public string Password { get; private set; }

        public string Encode()
        {
            string json = this.WriteJson();
            string result = CryptoUtil.Encrypt(json);

            return result;
        }

        public static CookieUserInfo FromEncodeString(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                    return null;

                string json = CryptoUtil.Decrypt(data);
                CookieUserInfo result = new CookieUserInfo();
                result.ReadJson(json);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
