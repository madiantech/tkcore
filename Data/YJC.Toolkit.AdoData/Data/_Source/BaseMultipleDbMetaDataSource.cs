using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public abstract class BaseMultipleDbMetaDataSource : BaseMultipleDbSource, ISupportMetaData
    {
        protected BaseMultipleDbMetaDataSource()
        {
        }

        protected BaseMultipleDbMetaDataSource(IEditDbConfig config)
            : base(config)
        {
            UseMetaData = config.UseMetaData;
        }

        #region ISupportMetaData 成员

        public bool CanUseMetaData(IPageStyle style)
        {
            if (TestPageStyleForMetaData(style))
                return UseMetaData;
            return false;
        }

        public virtual void SetMetaData(IPageStyle style, IMetaData metaData)
        {
            ITableSchemeEx scheme;
            if (TestPageStyleForMetaData(style))
            {
                scheme = metaData.GetTableScheme(MainResolver.TableName);
                if (scheme != null)
                {
                    MainResolver.ReadMetaData(scheme);
                    OnReadMetaData(MainResolver, style, scheme);
                }

                foreach (var childTable in ChildTables)
                {
                    scheme = metaData.GetTableScheme(childTable.Resolver.TableName);
                    if (scheme != null)
                    {
                        childTable.Resolver.ReadMetaData(scheme);
                        OnReadMetaData(childTable.Resolver, style, scheme);
                    }
                }

            }
        }

        #endregion

        public bool UseMetaData { get; set; }

        protected abstract bool TestPageStyleForMetaData(IPageStyle style);

        protected virtual void OnReadMetaData(TableResolver resolver, IPageStyle style, ITableSchemeEx scheme)
        {
        }
    }
}
