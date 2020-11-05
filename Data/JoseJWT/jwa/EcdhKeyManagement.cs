using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace YJC.Toolkit.JWT
{
    internal class EcdhKeyManagement : IKeyManagement
    {
        private string algIdHeader;

        public EcdhKeyManagement(bool isDirectAgreement)
        {
            algIdHeader = isDirectAgreement ? "enc" : "alg";
        }

        public virtual byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, string> header)
        {
            var cek = NewKey(cekSizeBits, key, header);
            var encryptedCek = Wrap(cek, key);

            return new[] { cek, encryptedCek };
        }

        private byte[] NewKey(int keyLength, object key, IDictionary<string, string> header)
        {
            var recieverPubKey = Ensure.Type<CngKey>(key, "EcdhKeyManagement alg expects key to be of CngKey type.");

            EccKey ephemeral = EccKey.Generate(recieverPubKey);

            IDictionary<string, string> epk = new Dictionary<string, string>();
            epk["kty"] = "EC";
            epk["x"] = Base64Url.Encode(ephemeral.X);
            epk["y"] = Base64Url.Encode(ephemeral.Y);
            epk["crv"] = Curve(recieverPubKey);

            //header["epk"] = epk;

            //return DeriveKey(header, keyLength, recieverPubKey, ephemeral.Key);
            return null;
        }

        public virtual byte[] Wrap(byte[] cek, object key)
        {
            return Arrays.Empty;
        }

        public virtual byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, string> header)
        {
            var privateKey = Ensure.Type<CngKey>(key, "EcdhKeyManagement alg expects key to be of CngKey type.");

            Ensure.Contains(header, new[] { "epk" }, "EcdhKeyManagement algorithm expects 'epk' key param in JWT header, but was not found");
            Ensure.Contains(header, new[] { algIdHeader }, "EcdhKeyManagement algorithm expects 'enc' header to be present in JWT header, but was not found");

            IDictionary<string, object> epk = new Dictionary<string, object>();// (IDictionary<string, object>)header["epk"];

            Ensure.Contains(epk, new[] { "x", "y", "crv" }, "EcdhKeyManagement algorithm expects 'epk' key to contain 'x','y' and 'crv' fields.");

            var x = Base64Url.Decode((string)epk["x"]);
            var y = Base64Url.Decode((string)epk["y"]);

            var externalPublicKey = EccKey.New(x, y, usage: CngKeyUsages.KeyAgreement);

            return DeriveKey(header, cekSizeBits, externalPublicKey, privateKey);
        }

        private byte[] DeriveKey(IDictionary<string, string> header, int cekSizeBits, CngKey externalPublicKey, CngKey privateKey)
        {
            byte[] enc = Encoding.UTF8.GetBytes((string)header[algIdHeader]);
            byte[] apv = header.ContainsKey("apv") ? Base64Url.Decode((string)header["apv"]) : Arrays.Empty;
            byte[] apu = header.ContainsKey("apu") ? Base64Url.Decode((string)header["apu"]) : Arrays.Empty;

            byte[] algorithmId = Arrays.Concat(Arrays.IntToBytes(enc.Length), enc);
            byte[] partyUInfo = Arrays.Concat(Arrays.IntToBytes(apu.Length), apu);
            byte[] partyVInfo = Arrays.Concat(Arrays.IntToBytes(apv.Length), apv);
            byte[] suppPubInfo = Arrays.IntToBytes(cekSizeBits);

            return ConcatKDF.DeriveKey(externalPublicKey, privateKey, cekSizeBits, algorithmId, partyVInfo, partyUInfo, suppPubInfo);
        }

        private string Curve(CngKey key)
        {
            if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP256) return "P-256";
            if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP384) return "P-384";
            if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP521) return "P-521";

            throw new ArgumentException("Unknown curve type " + key.Algorithm);
        }
    }
}