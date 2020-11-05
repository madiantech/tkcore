using System;
using System.IO;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal static class ConfigUtil
    {
        public static void Test()
        {
            string fileName = Path.Combine(BaseAppSetting.Current.XmlPath, "Default.xml");
            TestConfig config = new TestConfig();
            config.ReadXmlFromFile(fileName);

            var obj = config.GetFactoryDefaultObject("Source", "SingleDbSource");
            Console.WriteLine(obj.ToString());
        }

        private static void Test(string text)
        {
            //text += "==";
            byte[] data = Convert.FromBase64String(text);
            foreach (var bt in data)
            {
                Console.Write(bt.ToString("x") + " ");
            }
            Console.WriteLine();
        }

        public static void TestUrl()
        {
            Test("zd89xcqcy0g0pr7n2dlkhq");
            //Test("gn9sltzi8uw8qnuw6dxua");
            Test("wysuyle27kiltmttes83tw");
            Test("r2qn8f6do0skwhrhs3wocq");
            Test("tydgocwiw0ggyemn1dxuiq");
            // Test("");
        }
    }
}