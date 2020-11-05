using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [EasySearchConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2014-03-17",
        Author = "YJC", Description = "支持Tk5的标准代码表的EasySearch配置")]
    [ObjectContext]
    class Tk5CodeTableEasySearchConfig : BaseCodeTableEasySearchConfig
    {
        public const string BASE_CLASS = "Tk5CodeTableEasySearch";

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }
    }
}
