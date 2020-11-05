using YJC.Toolkit.Decoder;
using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    [EnumCodeTable(Author = "YJC", CreateDate = "2015-10-21", Description = "插件类型的代码表")]
    public enum PlugInType
    {
        [DisplayName("代码")]
        Code,

        [DisplayName("实例")]
        Instance,

        [DisplayName("配置")]
        Xml
    }
}