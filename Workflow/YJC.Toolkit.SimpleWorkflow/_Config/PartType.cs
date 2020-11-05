using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-12", Description = "部分类型代码表")]
    internal enum PartType
    {
        [DisplayName("百分比")]
        Percent,

        [DisplayName("绝对数目")]
        Count
    }
}