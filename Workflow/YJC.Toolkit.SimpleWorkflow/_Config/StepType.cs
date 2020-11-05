using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-12", Description = "步骤类型代码表")]
    public enum StepType
    {
        [DisplayName("未定义")]
        None = 0,

        [DisplayName("开始")]
        Begin = 1,

        [DisplayName("结束")]
        End = 2,

        [DisplayName("人工")]
        Manual = 3,

        [DisplayName("路由")]
        Route = 4,

        [DisplayName("自动")]
        Auto = 5,

        [DisplayName("聚合")]
        Merge = 6
    }
}