using System;
using System.Diagnostics;
using TestConsoleApp.Properties;
using TestJWT;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!ConsoleApp.Initialize())
                return;

            //RazorTest.TestUpdateData();
            //TestResource();
            //TestUtil.TestJWT();
            //TestEvalutor.TestEvalator();
            //ConfigUtil.TestUrl();
            //var byteArr = new byte[] { 0x6d, 0xda, 0xf1, 0xbe, 0 }; //bdruvpA
            //for (byte i = 0; i < 255; ++i)
            //{
            //    byteArr[4] = i;
            //    var s = Convert.ToBase64String(byteArr).TrimEnd('=');
            //    s = Uri.EscapeDataString(s);
            //    Console.WriteLine(s);
            //    Trace.WriteLine(s);
            //}
            //var data = Convert.FromBase64String("bdjshsa=");
            //Random rand = new Random((int)DateTime.Now.Ticks);
            //char[] data = new char[4];
            //for (int j = 0; j < 20; ++j)
            //{
            //    for (int i = 0; i < 4; ++i)
            //        data[i] = rand.Next(0, 10).ToString()[0];
            //    string url = "https://www.epanel.cn/evt/24563?u=13329911&t=14" + new string(data);
            //    Console.WriteLine(url);
            //}
            //TestUtil.Execute();
            //ProxyUtil.Hello();
            //TestLinq.Group();
            //TestUtil.TestDictionary();
            DateTime Start = new DateTime(1970, 1, 1) + TimeZoneInfo.Local.BaseUtcOffset;
            Console.Write(Start);
            Console.ReadKey();
        }

        private static void TestResource()
        {
            Resources.Culture = new System.Globalization.CultureInfo("en");
            Console.WriteLine(Resources.Hello);
        }
    }
}