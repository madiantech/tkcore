using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YJC.Toolkit.Sys;

namespace TestToolApp
{
    internal static class NodeTest
    {
        public static void Run()
        {
            //Environment.CurrentDirectory = @"E:\VS2019\webpack\webpack-demo\";
            ProcessStartInfo startInfo = new ProcessStartInfo("node")
            {
                Arguments = @"build.js",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = @"E:\VS2019\webpack\webpack-demo"
            };
            using (Process process = new Process() { StartInfo = startInfo })
            {
                process.OutputDataReceived += Process_OutputDataReceived;
                process.Start();
                Console.WriteLine(process.StandardOutput.ReadToEnd());
            }
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
