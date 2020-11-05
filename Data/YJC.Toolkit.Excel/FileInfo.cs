using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Excel
{
    internal class FileInfo
    {
        [SimpleAttribute]
        public string FileName { get; private set; }

        [SimpleAttribute]
        public int FileSize { get; private set; }

        [SimpleAttribute]
        public string ServerPath { get; private set; }

        [SimpleAttribute]
        public string ContentType { get; private set; }
    }
}
