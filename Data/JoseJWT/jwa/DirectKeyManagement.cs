using System.Collections.Generic;

namespace YJC.Toolkit.JWT
{
    internal class DirectKeyManagement : IKeyManagement
    {
        public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, string> header)
        {
            return new[] { Ensure.Type<byte[]>(key, "DirectKeyManagement alg expectes key to be byte[] array."), Arrays.Empty };
        }

        public byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, string> header)
        {
            Ensure.IsEmpty(encryptedCek, "DirectKeyManagement expects empty content encryption key.");

            return Ensure.Type<byte[]>(key, "DirectKeyManagement alg expectes key to be byte[] array.");
        }
    }
}