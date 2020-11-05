using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-13", Description = "步骤状态代码表")]
    public enum StepState
    {
        [DisplayName("未接收")]
        NotReceive = 0,

        [DisplayName("接收未打开")]
        ReceiveNotOpen = 1,

        [DisplayName("打开未处理")]
        OpenNotProcess = 2,

        [DisplayName("处理未发送")]
        ProcessNotSend = 3,

        [DisplayName("错误")]
        Mistake = 4
    }
}