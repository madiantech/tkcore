using System;
using YJC.Toolkit.JWT;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    public static class JWTUtil
    {
        //private static X509Certificate2 fCertificate = CreateCertificate();
        public const string COOKIE_NAME = "JwtToken";

        public const string DEFAULT_NAME = "JwtKey";
        private static string fSecretKey;

        public static string SecretKey
        {
            get
            {
                if (fSecretKey == null)
                    fSecretKey = DefaultUtil.GetSimpleValue(DEFAULT_NAME, ToolkitConst.TOOLKIT);

                return fSecretKey;
            }
        }

        //private static X509Certificate2 CreateCertificate()
        //{
        //    TkDebug.ThrowIfNoAppSetting();
        //    string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, "YJC.Toolkit2.pfx");
        //    X509Certificate2 cer = new X509Certificate2(fileName, "helloworld", (X509KeyStorageFlags)32); //, (X509KeyStorageFlags)32 | X509KeyStorageFlags.Exportable
        //    return cer;
        //}

        public static string CreateEncodingInfo(IUserInfo userInfo)
        {
            if (userInfo is JWTUserInfo jwtInfo)
                return EncodeToJwt(jwtInfo);
            else
            {
                if (userInfo.IsSupportTenant())
                    jwtInfo = new JWTTetantUserInfo(userInfo);
                else
                    jwtInfo = new JWTUserInfo(userInfo);
                return EncodeToJwt(jwtInfo);
            }
        }

        public static string EncodeToJwt(IHttpHeader info)
        {
            string json = info.WriteJson();
            //var publicKey = fCertificate.GetRSAPublicKey();
            string token = JWT.JWT.Encode(json, SecretKey, JweAlgorithm.PBES2_HS256_A128KW, JweEncryption.A256CBC_HS512);
            return token;
        }

        public static JWTUserInfo DecodeFromJwt(string token)
        {
            string json = JWT.JWT.Decode(token, SecretKey); //fCertificate.GetRSAPrivateKey());
            JWTTetantUserInfo result = json.ReadJson<JWTTetantUserInfo>();
            if (string.IsNullOrEmpty(result.TenantId?.ToString()))
            {
                JWTUserInfo info = new JWTUserInfo(result);
                return info;
            }
            else
                return result;
        }

        public static DateTime CalcValidTime()
        {
            DateTime nextDay = DateTime.Today.AddDays(1);
            DateTime eightHour = DateTime.Now.AddHours(8);

            return nextDay > eightHour ? eightHour : nextDay;
        }
    }
}