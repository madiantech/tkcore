using YJC.Toolkit.Collections;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Decoder
{
    internal class TenantStandardCodeTableScheme : StandardCodeTableScheme
    {
        public TenantStandardCodeTableScheme(string tableName)
            : base(tableName)
        {
            RegNameList<FieldItem> list = Fields.Convert<RegNameList<FieldItem>>();
            list.Add(TenantUtil.TENANT_FIELD);
        }
    }
}