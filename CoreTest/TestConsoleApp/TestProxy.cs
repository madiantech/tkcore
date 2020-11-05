using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    public class TestProxy<T> : DispatchProxy
    {
        private readonly Type fType;
        private readonly Dictionary<MethodInfo, string> fDictionary;

        public TestProxy()
        {
            fType = typeof(T);
            fDictionary = new Dictionary<MethodInfo, string>();
            var methods = fType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var method in methods)
            {
                string name = method.Name;
                fDictionary.Add(method, name);
            }
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            string name = fDictionary[targetMethod];
            Console.WriteLine($"Hello, invoke {name}");

            return null;
        }
    }
}