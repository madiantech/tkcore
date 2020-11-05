using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-12", Description = "计算类型代码表")]
    public enum CalculationType
    {
        [DisplayName("表达式")]
        Expression,

        [DisplayName("插件")]
        PlugIn
    }
}