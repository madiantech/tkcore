using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Weixin.Message;

namespace YJC.Toolkit.Weixin
{
    public static class WeUtil
    {
        private const int NONCE_LENGTH = 32;

        private readonly static char[] CHARS =
              "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        private readonly static WriteSettings DefaultXmlSettings = new WriteSettings
        {
            CloseInput = true,
            Encoding = ToolkitConst.UTF8,
            OmitHead = true
        };

        public static readonly QName QNAME_XML = QName.Get("xml");

        static WeUtil()
        {
            // 微信某些HTTPS地址存在权限问题
            ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        internal static string InteralCreateSignature(string raw)
        {
            using (var sha = new SHA1CryptoServiceProvider())
            {
                byte[] dataToHash = Encoding.ASCII.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                string hash = StringUtil.BinaryToHex(dataHashed, true);
                return hash;
            }
        }

        internal static string CreateSignature(params string[] stringList)
        {
            Array.Sort(stringList, WeixinStringComparer.Comparer);
            string raw = string.Join(string.Empty, stringList);

            return InteralCreateSignature(raw);
        }

        internal static EncodeReplyMessage EncodeReplyMessage(string timeStamp, string nonce,
            string token, string raw)
        {
            string msgSigature = CreateSignature(token, timeStamp, nonce, raw);
            if (msgSigature == null)
                return null;
            EncodeReplyMessage msg = new EncodeReplyMessage(raw, msgSigature, timeStamp, nonce);
            return msg;
        }

        public static bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string tmpStr = CreateSignature(token, timestamp, nonce);

            return tmpStr == signature;
        }

        public static string CreateAuthUrl(string appId, string url, AuthType authType, string state)
        {
            TkDebug.AssertArgumentNullOrEmpty(url, "url", null);

            string authUrl = string.Format(ObjectUtil.SysCulture, WeConst.AUTH_URL,
               appId, Uri.EscapeDataString(url),
               "snsapi_" + authType.ToString().ToLower(ObjectUtil.SysCulture), state ?? string.Empty);

            return authUrl;
        }

        //public static string GetSXCode(TkDbContext context, string province, string city,
        //    string defaultCode = "000000")
        //{
        //    const string sql = "SELECT MIN(CODE_VALUE) FROM CD_SX";
        //    IParamBuilder builder = SqlParamBuilder.CreateParamBuilder(
        //        SqlParamBuilder.CreateEqualSql(context, "CODE_WX_PROVINCE", TkDataType.String, province),
        //        SqlParamBuilder.CreateEqualSql(context, "CODE_WX_CITY", TkDataType.String, city));
        //    object value = DbUtil.ExecuteScalar(sql, context, builder);
        //    if (value == DBNull.Value)
        //        return defaultCode;
        //    return value.ToString();
        //}

        public static EncodeReplyMessage EncryptMsg(string appId, string token,
            string encodingAESKey, string replyMsg, string timeStamp, string nonce)
        {
            string raw = Cryptography.AesEncrypt(replyMsg, encodingAESKey, appId);

            return EncodeReplyMessage(timeStamp, nonce, token, raw);
        }

        public static string DecryptMsg(EncodeReceiveMessage message, string token,
            string encodingAESKey, string msgSignature, string timeStamp, string nonce)
        {
            string encryptMsg = message.Encrypt;

            //verify signature
            if (VerifySignature(token, timeStamp, nonce, encryptMsg, msgSignature))
            {
                string appId;
                string sMsg = Cryptography.AesDecrypt(encryptMsg, encodingAESKey, out appId);
                return sMsg;
            }
            return null;
        }

        public static bool VerifySignature(string token, string timeStamp, string nonce,
            string msgEncrypt, string sigture)
        {
            string hash = WeUtil.CreateSignature(token, timeStamp, nonce, msgEncrypt);
            return hash == sigture;
        }

        public static string Md5(string text)
        {
            TkDebug.AssertArgumentNullOrEmpty(text, "text", null);

            using (MD5 md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                return StringUtil.BinaryToHex(result, false);
            }
        }

        public static string CreateNonceStr()
        {
            char[] data = new char[NONCE_LENGTH];
            int srcLength = CHARS.Length;
            Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            for (int i = 0; i < NONCE_LENGTH; ++i)
                data[i] = CHARS[rand.Next(srcLength)];

            return new string(data);
        }

        public static string ToXml(object obj)
        {
            string xml = obj.WriteXml(DefaultXmlSettings, QNAME_XML);
            return xml;
        }
    }
}