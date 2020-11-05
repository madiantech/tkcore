using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-13", Description = "流转类型代码表")]
    public enum FlowAction
    {
        [DisplayName("流转")]
        Flow,

        [DisplayName("回退")]
        Back
    }
}