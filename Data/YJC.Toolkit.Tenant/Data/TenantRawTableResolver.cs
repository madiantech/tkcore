using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TenantRawTableResolver : TableResolver
    {
        private IFieldInfo fTenantId;

        public TenantRawTableResolver(ITableScheme scheme, IDbDataSource source)
            : base(scheme, source)
        {
        }

        public TenantRawTableResolver(string tableName, IDbDataSource source)
            : base(tableName, source)
        {
        }

        public TenantRawTableResolver(string tableName, string keyFields, IDbDataSource source)
            : base(tableName, keyFields, source)
        {
        }

        public TenantRawTableResolver(string tableName, string keyFields, string fields, IDbDataSource source)
            : base(tableName, keyFields, fields, source)
        {
        }

        public IFieldInfo TenantId
        {
            get
            {
                if (fTenantId == null)
                {
                    fTenantId = TenantUtil.GetTenantId(this, TenantIdNickName);
                }
                return fTenantId;
            }
        }

        public string TenantIdNickName { get; set; }

        protected override IParamBuilder CreateFilterBuilder()
        {
            return TenantUtil.GetTenantParamBuilder(Context, TenantId);
        }

        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            if (e.Status == UpdateKind.Insert)
                e.Row[TenantId.NickName] = BaseGlobalVariable.Current.UserInfo.GetTenantValue(true);
        }
    }
}