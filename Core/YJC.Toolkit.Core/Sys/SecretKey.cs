using System.Security.Cryptography;
using System.Text;

namespace YJC.Toolkit.Sys
{
    public sealed class SecretKey : ISecretKey
    {
        private SecretKey()
        {
            Mode = CipherMode.CBC;
            Padding = PaddingMode.PKCS7;
        }

        public SecretKey(string key, byte[] iv)
            : this()
        {
            TkDebug.AssertArgumentNullOrEmpty(key, "key", null);
            TkDebug.AssertArgumentNull(iv, "iv", null);

            InternalKey = key;
            IV = iv;
        }

        #region ISecretKey 成员

        public byte[] Key
        {
            get
            {
                return Encoding.UTF8.GetBytes(InternalKey);
            }
        }

        [SimpleAttribute]
        public byte[] IV { get; private set; }

        #endregion

        [SimpleAttribute(LocalName = "Key")]
        private string InternalKey { get; set; }

        [SimpleAttribute(DefaultValue = CipherMode.CBC)]
        public CipherMode Mode { get; private set; }

        [SimpleAttribute(DefaultValue = PaddingMode.PKCS7)]
        public PaddingMode Padding { get; private set; }
    }
}
