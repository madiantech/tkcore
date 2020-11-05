using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 备案证明文件
    {

        [SimpleElement(LocalName = "附加", Order = 10)]
        public string 附加 { get; private set; }

        [SimpleElement(LocalName = "备案证明文件名称", Order = 20)]
        public string 备案证明文件名称 { get; private set; }

        [SimpleElement(LocalName = "备案证明文件编号", Order = 30)]
        public string 备案证明文件编号 { get; private set; }
    }
}