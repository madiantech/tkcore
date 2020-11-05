using System;
using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleRight
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2015-09-21", UseIntValue = false,
        Description = "操作符所在页面的代码表")]
    [Flags]
    public enum OperatorPage
    {
        [DisplayName("列表")]
        List = 0x1,
        [DisplayName("详细")]
        Detail = 0x2,
        [DisplayName("全部")]
        All = 0x3
    }
}
