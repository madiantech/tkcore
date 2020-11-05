using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    [CodeTableConfig(NamespaceType = NamespaceType.Toolkit, CreateDate = "2018-05-30",
        Author = "YJC", Description = "支持TenantId的标准代码表，除了标准的五个字段外，还需要有CODE_TENANT_ID")]
    [ObjectContext]
    internal class TenantStandardCodeTableConfig : StandardCodeTableConfig
    {
        public const string TENANT_BASE_CLASS = "TenantStandard";

        public override string BaseClass
        {
            get
            {
                return TENANT_BASE_CLASS;
            }
        }
    }
}