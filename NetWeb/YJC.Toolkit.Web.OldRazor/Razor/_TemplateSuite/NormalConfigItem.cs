using System;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Razor
{
    internal class NormalConfigItem
    {

        [SimpleAttribute(DefaultValue = true)]
        public bool UseNamePath { get; private set; }

        [SimpleAttribute]
        public string List { get; private set; }

        [SimpleAttribute]
        public string Edit { get; private set; }

        [SimpleAttribute]
        public string Detail { get; private set; }

        [SimpleAttribute]
        public string DetailList { get; private set; }

        [SimpleAttribute]
        public string Tree { get; private set; }

        [SimpleAttribute]
        public string TreeDetail { get; private set; }

        [SimpleAttribute]
        public string MultiEdit { get; private set; }

        [SimpleAttribute]
        public string MultiDetail { get; private set; }

        [SimpleAttribute]
        public string MultiTreeDetail { get; private set; }
    }
}