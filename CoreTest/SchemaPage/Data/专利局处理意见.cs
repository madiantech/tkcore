using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 专利局处理意见
    {

        [SimpleElement(LocalName = "意见内容", Order = 10)]
        public string 意见内容 { get; private set; }

        [ObjectElement(LocalName = "提出日期", Order = 20)]
        public DateClass 提出日期 { get; private set; }
    }
}