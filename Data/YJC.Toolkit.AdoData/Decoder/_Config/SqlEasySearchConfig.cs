using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [EasySearchConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-05-23",
        Author = "YJC", Description = "以Sql结果集作为数据源的EasySearch")]
    [ObjectContext]
    internal class SqlEasySearchConfig : BaseEasySearchConfig
    {
        public const string BASE_CLASS = "SqlEasySearch";

        [SimpleElement(NamespaceType.Toolkit, Required = true)]
        public string Sql { get; private set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }

        public override ITableSchemeEx CreateScheme()
        {
            return null;
        }
    }
}