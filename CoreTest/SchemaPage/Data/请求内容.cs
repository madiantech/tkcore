using System;
using YJC.Toolkit.Sys;
using Toolkit.SchemaSuite;

namespace Toolkit.SchemaSuite.Data
{
    public class 请求内容
    {

        [ObjectElement(LocalName = "专利局发出通知的日期", Order = 10)]
        public DateClass 专利局发出通知的日期 { get; private set; }

        [SimpleElement(LocalName = "专利局发出通知的名称", Order = 20)]
        public string 专利局发出通知的名称 { get; private set; }

        [SimpleElement(LocalName = "发文序号", Order = 30)]
        public string 发文序号 { get; private set; }
    }
}