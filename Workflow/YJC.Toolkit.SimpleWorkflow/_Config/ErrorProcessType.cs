using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-12", Description = "错误处理类型代码表")]
    public enum ErrorProcessType
    {
        [DisplayName("终止")]
        Abort,

        [DisplayName("重试")]
        Retry
    }
}