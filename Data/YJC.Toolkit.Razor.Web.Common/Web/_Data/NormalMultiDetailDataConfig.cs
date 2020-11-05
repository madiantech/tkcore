using System.Collections.Generic;
using YJC.Toolkit.Razor;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Web
{
    [RazorDataConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-02-06",
        Author = "YJC", Description = "匹配NormalMultiDetail模板使用的数据")]
    internal class NormalMultiDetailDataConfig : NormalDetailDataConfig
    {
        [ObjectElement(NamespaceType.Toolkit, IsMultiple = true, LocalName = "TableData")]
        public List<SingleTableDetailData> TableDatas { get; private set; }

        public override object CreateObject(params object[] args)
        {
            return new NormalMultiDetailData(this);
        }
    }
}