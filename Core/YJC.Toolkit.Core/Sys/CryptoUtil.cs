using System;
using System.Security.Cryptography;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public static class CryptoUtil
    {
        private static byte[] TransfromData(ISecretKey key, byte[] source, bool encrpty)
        {
            TkDebug.AssertArgumentNull(key, "key", null);
            TkDebug.AssertArgumentNull(source, "source", null);

            RijndaelManaged managed = new RijndaelManaged();
            using (managed)
            {
                SecretKey secretKey = key as SecretKey;
                if (secretKey != null)
                {
                    managed.Mode = secretKey.Mode;
                    managed.Padding = secretKey.Padding;
                }
                ICryptoTransform transform;
                if (encrpty)
                    transform = managed.CreateEncryptor(GetHashKey(key.Key), key.IV);
                else
                    transform = managed.CreateDecryptor(GetHashKey(key.Key), key.IV);

                using (transform)
                {
                    return transform.TransformFinalBlock(source, 0, source.Length);
                }
            }
        }

        private static byte[] GetHashKey(byte[] data)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            using (x)
            {
                return x.ComputeHash(data);
            }
        }

        public static byte[] Encrypt(ISecretKey key, byte[] source)
        {
            return TransfromData(key, source, true);
        }

        public static byte[] Decrypt(ISecretKey key, byte[] source)
        {
            return TransfromData(key, source, false);
        }

        public static byte[] Encrypt(byte[] source)
        {
            TkDebug.ThrowIfNoAppSetting();

            return Encrypt(BaseAppSetting.Current.SecretKey, source);
        }

        public static byte[] Decrypt(byte[] source)
        {
            TkDebug.ThrowIfNoAppSetting();

            return Decrypt(BaseAppSetting.Current.SecretKey, source);
        }

        public static string Encrypt(ISecretKey key, string source)
        {
            TkDebug.AssertArgumentNullOrEmpty(source, "source", null);

            byte[] sourceData = Encoding.UTF8.GetBytes(source);
            byte[] result = Encrypt(key, sourceData);
            return Convert.ToBase64String(result);
        }

        public static string Decrypt(ISecretKey key, string source)
        {
            TkDebug.AssertArgumentNullOrEmpty(source, "source", null);

            byte[] srcData = Convert.FromBase64String(source);
            byte[] result = Decrypt(key, srcData);
            return Encoding.UTF8.GetString(result);
        }

        public static string Encrypt(string source)
        {
            TkDebug.ThrowIfNoAppSetting();

            return Encrypt(BaseAppSetting.Current.SecretKey, source);
        }

        public static string Decrypt(string source)
        {
            TkDebug.ThrowIfNoAppSetting();

            return Decrypt(BaseAppSetting.Current.SecretKey, source);
        }
    }
}
