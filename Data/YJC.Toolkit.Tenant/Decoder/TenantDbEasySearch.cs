using YJC.Toolkit.Data;
using YJC.Toolkit.MetaData;
using YJC.Toolkit.Sys;
using System.Data;

namespace YJC.Toolkit.Decoder
{
    public class TenantDbEasySearch : Tk5DbEasySearch
    {
        private IFieldInfo fTenantId;

        public TenantDbEasySearch(Tk5DataXml scheme)
            : base(scheme)
        {
        }

        internal TenantDbEasySearch(Tk5TenantEasySearchConfig config)
            : base(config)
        {
            TenantIdNickName = config.TenantIdNickName;
        }

        protected override IParamBuilder CreateAdditionCondition(TkDbContext context,
            IFieldInfoIndexer indexer)
        {
            IParamBuilder builder = base.CreateAdditionCondition(context, indexer);
            IParamBuilder tenant = TenantUtil.GetTenantParamBuilder(context, TenantId);
            return ParamBuilder.CreateParamBuilder(builder, tenant);
        }

        protected override DataRow FetchRow(string code, TkDbContext context, DataSet dataSet)
        {
            TempSource source = new TempSource(dataSet, context);
            TableSelector selector = new TableSelector(ProxyScheme, source);
            using (selector)
            {
                IParamBuilder builder = selector.CreateParamBuilder(null, new string[] { "Value" }, code);
                builder = ParamBuilder.CreateParamBuilder(builder,
                    TenantUtil.GetTenantParamBuilder(context, TenantId));
                return selector.TrySelectRow(builder);
            }
        }

        public string TenantIdNickName { get; set; }

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
    }
}