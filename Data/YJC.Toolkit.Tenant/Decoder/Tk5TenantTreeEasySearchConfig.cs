using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [EasySearchConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-06-27",
        Author = "YJC", Description = "在Tk5的DataXml基础上，支持Tenant的TreeEasySearch配置")]
    [ObjectContext]
    internal class Tk5TenantTreeEasySearchConfig : Tk5TenantEasySearchConfig
    {
        public const string TREE_BASE_CLASS = "Tk5TenantTreeEasySearch";

        public override string BaseClass
        {
            get
            {
                return TREE_BASE_CLASS;
            }
        }
    }
}