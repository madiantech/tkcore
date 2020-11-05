using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-14", Description = "相对用户类型代码表")]
    public enum RelativeUserType
    {
        [DisplayName("接收人")]
        Receiver = 0,

        [DisplayName("处理人")]
        Processor = 1,

        [DisplayName("发送人")]
        Sender = 2
    }
}