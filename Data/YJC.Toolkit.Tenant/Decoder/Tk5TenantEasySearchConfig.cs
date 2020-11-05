using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [EasySearchConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-05-22",
        Author = "YJC", Description = "在Tk5的DataXml基础上，支持Tenant的EasySearch配置")]
    [ObjectContext]
    internal class Tk5TenantEasySearchConfig : BaseEasySearchConfig
    {
        public const string BASE_CLASS = "Tk5TenantEasySearch";

        [SimpleAttribute]
        public string TenantIdNickName { get; set; }

        [SimpleAttribute(Required = true)]
        public string DataXml { get; protected set; }

        public override string BaseClass
        {
            get
            {
                return BASE_CLASS;
            }
        }

        public override ITableSchemeEx CreateScheme()
        {
            return Tk5DataXml.Create(DataXml);
        }
    }
}