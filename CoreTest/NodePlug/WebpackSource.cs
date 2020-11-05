using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using YJC.Toolkit.Sys;
using YJC.Toolkit.Data;
using System.Diagnostics;
using System.IO;

namespace NodePlug
{
    [Source(Author = "Administrator", CreateDate = "2019/12/11 16:37:47",
        Description = "数据源")]
    internal class WebpackSource : ISource
    {
        public WebpackSource()
        {
        }

        public OutputData DoAction(IInputData input)
        {
            string dir = Path.Combine(BaseAppSetting.Current.SolutionPath, "wwwroot", "htmltest", "webpacktest");
            ProcessStartInfo startInfo = new ProcessStartInfo("node")
            {
                Arguments = @"build.js",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                WorkingDirectory = dir
            };
            using (Process process = new Process() { StartInfo = startInfo })
            {
                process.Start();
                Console.WriteLine(process.StandardOutput.ReadToEnd());
            }
            return OutputData.Create("/htmltest/webpacktest/dist/index.html");
        }
    }
}
