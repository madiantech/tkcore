using System;
using System.Collections.Generic;
using System.Text;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal class Hello
    {
        public string World { get; set; }
    }

    internal static class TestEvalutor
    {
        public static void TestMember()
        {
            Hello hello = new Hello() { World = "Hello" };
            string value = hello.MemberValue("World").ToString();
            Console.WriteLine(value);
        }

        public static void TestEvalator()
        {
            string expr = "DateTime.Today";
            var value = EvaluatorUtil.Execute<DateTime>(expr);
            Console.WriteLine(value);
        }
    }
}