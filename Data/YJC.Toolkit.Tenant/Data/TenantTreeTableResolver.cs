using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    public class TenantTreeTableResolver : Tk5TreeTableResolver
    {
        private IFieldInfo fTenantId;

        public TenantTreeTableResolver(string fileName, IDbDataSource source)
            : base(fileName, source)
        {
        }

        public TenantTreeTableResolver(string fileName, string tableName, IDbDataSource source)
            : base(fileName, tableName, source)
        {
        }

        public TenantTreeTableResolver(Tk5DataXml dataXml, IDbDataSource source)
            : base(dataXml, source)
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

        public override ITree CreateTree()
        {
            NormalDbTree tree = base.CreateTree().Convert<NormalDbTree>();
            tree.CustomCondition = CreateFilterBuilder();
            return tree;
        }

        protected override void OnUpdatingRow(UpdatingEventArgs e)
        {
            base.OnUpdatingRow(e);

            if (e.Status == UpdateKind.Insert)
                e.Row[TenantId.NickName] = BaseGlobalVariable.Current.UserInfo.GetTenantValue(true);
        }
    }
}