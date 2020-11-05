using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace TestToolApp
{
    internal static class WorkThreadTest
    {
        public static int Add(int a, int b)
        {
            return a + b;
        }

        public static void ProcessResult(IAsyncResult ar)
        {
            var result = BaseGlobalVariable.Current.EndInvoke(ar);
            int i = result.Value<int>();
            Console.WriteLine(i);
        }

        public static void TestWorkThread()
        {
            BaseGlobalVariable.Current.BeginInvoke(ProcessResult, new Func<int, int, int>(Add),
                new object[] { 10, 20 }, null);
        }
    }
}