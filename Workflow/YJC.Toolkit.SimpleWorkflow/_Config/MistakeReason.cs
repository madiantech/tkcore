using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-12", Description = "错误原因代码表")]
    internal enum MistakeReason
    {
        [DisplayName("无")]
        None = 0,

        [DisplayName("插件错误")]
        PlugInError = 1,

        [DisplayName("人员错误")]
        NoActor = 2,

        [DisplayName("路由错误")]
        NoRoute = 3
    }
}