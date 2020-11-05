using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Data;
using YJC.Toolkit.Sys;

namespace TestConsoleApp
{
    internal class TestConfig : BaseDefaultValue
    {
        public TestConfig()
        {
            AddSection("Source", SourceConfigFactory.REG_NAME);
        }
    }
}