using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class CustomConfigItem
    {

        [SimpleAttribute(DefaultValue = true)]
        public bool UseNamePath { get; private set; }

        [TextContent]
        public string Content { get; private set; }
    }
}