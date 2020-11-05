using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal static class ProxyUtil
    {
        public static void Hello()
        {
            var ser = DispatchProxy.Create<IObjectSerializer, TestProxy<IObjectSerializer>>();
            ser.Write(null, null, null, null, null, null, null);

            var obj2 = DispatchProxy.Create<IAuthor, TestProxy<IAuthor>>();
            Console.WriteLine(obj2.Author);
        }
    }
}