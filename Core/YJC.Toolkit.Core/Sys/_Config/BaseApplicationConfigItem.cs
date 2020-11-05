using System;
using System.Globalization;

namespace YJC.Toolkit.Sys
{
    internal class BaseApplicationConfigItem
    {
        [SimpleAttribute]
        public string Path { get; protected set; }

        [SimpleAttribute]
        public string AppPath { get; protected set; }

        [SimpleAttribute]
        public string PlugInPath { get; protected set; }

        [SimpleAttribute]
        public int CommandTimeout { get; protected set; }

        [SimpleAttribute]
        public CultureInfo Culture { get; protected set; }

        [SimpleAttribute]
        public TimeSpan CacheTime { get; protected set; }

        [SimpleAttribute]
        public bool UseCache { get; protected set; }
    }
}
