using System;
using YJC.Toolkit.Sys;

namespace TestIM
{
    public class ToolkitFixture : IDisposable
    {
        public ToolkitFixture()
        {
            TestApp.Initialize(@"");
            // 其他全局初始化代码，只做一次
        }

        public void Dispose()
        {
            // 全局释放代码，只做一次
        }
    }
}