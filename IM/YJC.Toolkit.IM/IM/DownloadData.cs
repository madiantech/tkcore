using System;
using System.Collections.Generic;
using System.Linq;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.IM
{
    public class DownloadData : BaseResult
    {
        internal DownloadData()
        {
        }

        public byte[] FileData { get; internal set; }
    }
}