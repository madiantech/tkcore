using System;
using YJC.Toolkit.Sys;

namespace WfConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!ConsoleApp.Initialize())
                return;

            WorkflowModelTest.UpdateModel();
            //WorkflowModelTest.TestSampleWf3();
            Console.ReadKey();
        }
    }
}