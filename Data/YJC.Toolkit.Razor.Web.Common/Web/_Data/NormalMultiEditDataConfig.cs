using System.Collections.Generic;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2016-07-28",
        Author = "YJC", Description = "匹配NormalMultiEdit模板使用的数据")]
    internal class NormalMultiEditDataConfig : NormalEditDataConfig
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "TableData")]
        public List<SingleTableEditData> TableDatas { get; private set; }

        public override object CreateObject(params object[] args)
        {
            return new NormalMultiEditData(this);
        }
    }
}