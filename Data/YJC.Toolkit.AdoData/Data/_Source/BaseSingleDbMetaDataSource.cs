using YJC.Toolkit.MetaData;

namespace YJC.Toolkit.Data
{
    public abstract class BaseSingleDbMetaDataSource : BaseSingleDbSource, ISupportMetaData
    {
        protected BaseSingleDbMetaDataSource()
        {
        }

        protected BaseSingleDbMetaDataSource(IEditDbConfig config)
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
            if (TestPageStyleForMetaData(style))
            {
                ITableSchemeEx scheme = metaData.GetTableScheme(MainResolver.TableName);
                if (scheme != null)
                {
                    MainResolver.ReadMetaData(scheme);
                    OnReadMetaData(MainResolver, style, scheme);
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
