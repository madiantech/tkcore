using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 分类附件
    {

        [SimpleAttribute(LocalName = "顺序")]
        public string 顺序 { get; private set; }

        [SimpleElement(LocalName = "附件类型", Order = 10)]
        public string 附件类型 { get; private set; }

        [SimpleElement(LocalName = "附件名称", Order = 20)]
        public string 附件名称 { get; private set; }

        [SimpleElement(LocalName = "属性", Order = 30)]
        public string 属性 { get; private set; }

        [SimpleElement(LocalName = "文件名称", Order = 40)]
        public string 文件名称 { get; private set; }
    }
}