using YJC.Toolkit.Sys;

namespace YJC.Toolkit.Data
{
    [SourceConfig(NamespaceType = NamespaceType.Toolkit, Author = "YJC",
        CreateDate = "2014-09-18", Description = "提供对多表的Delete数据源")]
    class MultipleDbDeleteSourceConfig : BaseMultipleConfig, IEditDbConfig
    {
        public override ISource CreateObject(params object[] args)
        {
            return new MultipleDbDeleteSource(this);
        }

        #region IEditDbConfig 成员

        public bool UseMetaData
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
