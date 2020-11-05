using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Right
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2015-09-17",
        Description = "操作符位置的代码表", UseIntValue = false)]
    public enum OperatorPosition
    {
        [DisplayName("每列")]
        Row,
        [DisplayName("全局")]
        Global
    }
}
