using RazorTest;
using System;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace TestToolApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ToolApp.Initialize(true);
            ToolApp.AddContextConfig(DbContextConfig.Create("Hello", "SQL Server2005", "SQL Server",
                "USER ID = sa; PASSWORD = windows95; INITIAL CATALOG = MituCXCS; DATA SOURCE = localhost; CONNECT TIMEOUT = 30"));

            Console.WriteLine("Hello World!");
            //XmlTestUtil.Hello();
            //QuickTest.TestUpdateData();
            //WorkThreadTest.TestWorkThread();
            NodeTest.Run();
            Console.ReadKey();

            ToolApp.FinalizeApp();
        }
    }
}