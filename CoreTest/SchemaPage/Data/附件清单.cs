using System;
using System.Collections.Generic;    
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 附件清单
    {

        [ObjectElement(IsMultiple = true, ObjectType = typeof(分类附件), LocalName = "分类附件", Order = 10)]
        public List<分类附件> 分类附件List { get; private set; }

        [ObjectElement(LocalName = "备案证明文件", Order = 20)]
        public 备案证明文件 备案证明文件 { get; private set; }
    }
}