using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using TestConsoleApp;
using YJC.Toolkit.JWT;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

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
            string fileName = @"E:\VS2018\Toolkit5.5\YJC.Toolkit2.pfx";
            X509Certificate2 cer = new X509Certificate2(fileName, "helloworld", X509KeyStorageFlags.UserKeySet | (X509KeyStorageFlags)32);
            //string fileName = @"E:\VS2018\Toolkit5.5\Toolkit.cer";
            //X509Certificate2 cer = new X509Certificate2(fileName);
            var publicKey = cer.GetRSAPublicKey();
            string token = JWT.Encode(json, publicKey, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);

            string json1 = JWT.Decode(token, cer.GetRSAPrivateKey());
            Console.WriteLine(token);
        }

        public static void Execute(string workDir, string argument)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("node")
            {
                Arguments = argument,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = workDir
            };
            using (Process process = new Process() { StartInfo = startInfo })
            {
                process.Start();
                process.WaitForExit();
                Console.WriteLine(process.StandardOutput.ReadToEnd());
                Console.WriteLine(process.StandardError.ReadToEnd());
            }
        }

        public static void Execute()
        {
            Execute(@"E:\VS2019\Toolkit5.5\CoreWebTest\TestWeb\Xml\estemp\vuedemo2\Modules\xml\Test\TestPart2", "build.js");
            //Execute(@"E:\VS2019\Toolkit5.5\CoreWebTest\TestWeb\Xml\estemp\vuedemo\Modules\xml\Test\TestPart", "build.js");
        }

        public static void ParseUrl()
        {
            string url = "/c/xml/list/User/Hello";
            string[] data = url.Split('/');
            var result = (Parser: data[1], Style: data[3].Value<PageStyleClass>(), Source: data[4]);
            Console.WriteLine(result);
        }

        public static void TestDictionary()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("测试", "数据");
            data.Add("日期", new DateStruct(DateTime.Today));

            string json = data.WriteJson();
            Console.WriteLine(json);
            string xml = data.WriteXml();
            Console.WriteLine(xml);
        }
    }
}