using System;
using System.Security.Cryptography;
using System.Text;

namespace YJC.Toolkit.Right
{
    internal class PasswordProvider : IPasswordProvider
    {
        internal PasswordProvider()
        {
        }

        #region IPasswordProvider 成员

        public string Format(string password)
        {
            return Encrypt(password, password);
        }

        public int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 3;
            }
        }

        public int MinRequiredPasswordLength
        {
            get
            {
                return 6;
            }
        }

        public int ValidateStrength(string password)
        {
            return 0;
        }

        #endregion IPasswordProvider 成员

        #region 加密方法

        private static string Encrypt(string toEncrypt, string key)
        {
            if (string.IsNullOrEmpty(toEncrypt))
            {
                return string.Empty;
            }

            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            TripleDESCryptoServiceProvider tdes = GetTripleDES(key);

            byte[] resultArray = CryptoTransform(tdes.CreateEncryptor(), toEncryptArray);

            tdes.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        private static TripleDESCryptoServiceProvider GetTripleDES(string key)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

            byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider()
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            return tdes;
        }

        private static byte[] CryptoTransform(ICryptoTransform cTransform, byte[] toEncryptArray)
        {
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }

        #endregion 加密方法
    }
}