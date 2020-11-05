using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace TestWfData
{
    internal class UserConfig
    {
        [ComplexContent]
        public string Content { get; private set; }
    }
}