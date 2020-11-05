using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.SimpleWorkflow
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2017-05-13", Description = "传输类型代码表")]
    internal enum TransferType
    {
        [DisplayName("所有")]
        All = 1,

        [DisplayName("模式")]
        Define = 2,

        [DisplayName("单个实例")]
        Instance = 3
    }
}