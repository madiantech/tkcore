using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [EasySearchConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-08-21",
        Author = "YJC", Description = "Tk5以Level0模式的树形标准代码表的EasySearch配置")]
    [ObjectContext]
    class Tk5Level0CodeTableEasySearchConfig : BaseLevelCodeTableEasySearchConfig
    {
        public const string BASE_CLASS = "Tk5Level0CodeTableEasySearch";

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}
