using System;
using System.Security.Cryptography.X509Certificates;
using YJC.Toolkit.JWT;

namespace TestJWT
{
    internal static class TestUtil
    {
        public static void TestCerFile()
        {
            string fileName = @"E:\VS2018\Toolkit5.5\Toolkit.cer";
            X509Certificate2 cer = new X509Certificate2(fileName);
            var key = cer.PublicKey;
            Console.WriteLine(key.ToString());
        }

        public static void TestJWT()
        {
            string json = "{ 'firstName': 'Brett', 'lastName':'McLaughlin', 'email': 'aaaa' }";
            string fileName = @"E:\VS2018\Toolkit5.5\hello.pfx";
            X509Certificate2 cer = new X509Certificate2(fileName, "hello12345");
            //string fileName = @"E:\VS2018\Toolkit5.5\Toolkit.cer";
            //X509Certificate2 cer = new X509Certificate2(fileName);
            var publicKey = cer.GetRSAPublicKey();
            //string token = JWT.Encode(json, publicKey, JweAlgorithm.RSA1_5, JweEncryption.A128CBC_HS256, JweCompression.DEF);
            string token = JWT.Encode(json, "top secret", JweAlgorithm.PBES2_HS256_A128KW, JweEncryption.A256CBC_HS512);

            string json1 = JWT.Decode(token, "top secret");
            Console.WriteLine(token);
            Console.WriteLine(json1);
        }
    }
}