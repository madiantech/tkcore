using System;
using YJC.Toolkit.Sys;

namespace TestJWT
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ToolApp.Initialize(false);

            TestUtil.TestJWT();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}